namespace EventHorizon.Zone.System.Server.Scripts.Load
{
    using EventHorizon.Observer.Model;
    using EventHorizon.Observer.State;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.Model.Generated;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Model;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Register;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Remove;
    using EventHorizon.Zone.System.Server.Scripts.Run.Model;
    using EventHorizon.Zone.System.Server.Scripts.State;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Linq;
    using global::System.Reflection;
    using global::System.Runtime.Loader;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class LoadNewServerScriptAssemblyCommandHandler
        : IRequestHandler<LoadNewServerScriptAssemblyCommand, StandardCommandResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly ServerScriptDetailsRepository _detailsRepository;
        private readonly ServerScriptRepository _scriptRepository;
        private readonly ObserverState _observerState;
        private readonly ServerScriptServices _serverScriptServices;

        public LoadNewServerScriptAssemblyCommandHandler(
            ILogger<LoadNewServerScriptAssemblyCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo,
            ServerScriptDetailsRepository detailsRepository,
            ServerScriptRepository scriptRepository,
            ObserverState observerState,
            ServerScriptServices serverScriptServices
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
            _detailsRepository = detailsRepository;
            _scriptRepository = scriptRepository;
            _observerState = observerState;
            _serverScriptServices = serverScriptServices;
        }

        public async Task<StandardCommandResult> Handle(
            LoadNewServerScriptAssemblyCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                using var scriptsAssemblyStream = File.OpenRead(
                    Path.Combine(
                        _serverInfo.GeneratedPath,
                        GeneratedServerScriptsResultModel.SCRIPTS_ASSEMBLY_FILE_NAME
                    )
                );
                // Load Assembly into Default context
                var assembly = AssemblyLoadContext.Default.LoadFromStream(
                    scriptsAssemblyStream
                );

                // Update ServerScript Details and Add ServerScript from DLL
                var scriptTypeList = GetScriptTypesList(
                    assembly
                );

                var newScriptList = new List<(string id, ServerScriptDetails scriptDetails, ServerScript serverScript)>();
                foreach (var scriptFullName in scriptTypeList)
                {
                    var scriptInstance = assembly.CreateInstance(
                        scriptFullName
                    );
                    if (scriptInstance is ServerScript serverScript)
                    {
                        // Update ServerScriptDetails for each ServerScript
                        var id = serverScript.Id;
                        var tags = serverScript.Tags;
                        var existingDetails = _detailsRepository.Find(
                            id
                        );
                        var updatedDetails = new ServerScriptDetails(
                            id,
                            existingDetails.Hash,
                            existingDetails.FileName,
                            existingDetails.Path,
                            existingDetails.ScriptString,
                            tags
                        );

                        newScriptList.Add((
                            id,
                            updatedDetails,
                            serverScript
                        ));
                    }
                }

                if (newScriptList.Any())
                {
                    foreach (var script in _scriptRepository.All)
                    {
                        if (script is ObserverBase observerScript)
                        {
                            _observerState.Remove(
                                observerScript
                            );
                        }

                        if (script is ScriptedBackgroundTask backgroundTask)
                        {
                            _logger.LogWarning(
                                "TODO: Send RemoveScriptedBackgroundTaskCommand for '{BackgroundTaskId}'",
                                backgroundTask.TaskId
                            );
                            await _mediator.Send(
                                new RemoveScriptedBackgroundTaskCommand(
                                    backgroundTask.TaskId
                                ),
                                cancellationToken
                            );
                        }

                        if (script is IDisposable disposableScript)
                        {
                            disposableScript.Dispose();
                        }
                    }

                    // Clear current scripts from repository
                    _scriptRepository.Clear();
                }

                foreach (var (id, updatedDetails, serverScript) in newScriptList)
                {
                    _detailsRepository.Add(
                        id,
                        updatedDetails
                    );

                    // Add Script to Repository
                    _scriptRepository.Add(
                        serverScript
                    );

                    // Check if Observer
                    if (serverScript is ObserverBase observerScript)
                    {
                        // Track Observer Scripts
                        _observerState.Register(
                            observerScript
                        );
                        await serverScript.Run(
                            _serverScriptServices,
                            new StandardServerScriptData(
                                null
                            )
                        );
                    }

                    // Check if Background Task
                    if (serverScript is ScriptedBackgroundTask backgroundTask)
                    {
                        // Register new Background Task
                        await _mediator.Send(
                            new RegisterNewScriptedBackgroundTaskCommand(
                                backgroundTask
                            ),
                            cancellationToken
                        );
                        await serverScript.Run(
                            _serverScriptServices,
                            new StandardServerScriptData(
                                null
                            )
                        );
                    }
                }

                return new();

            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to Load Server Assembly."
                );
                return new(
                    ServerScriptsErrorCodes.SERVER_SCRIPT_FAILED_LOADING_ASSEMBLY_ERROR_CODE
                );
            }
        }

        private static IEnumerable<string> GetScriptTypesList(
            Assembly assembly
        ) => assembly
            .GetTypes()
            .Where(
                a => (a.FullName?.StartsWith("css_root+") ?? false)
                    && a.IsAssignableTo(typeof(ServerScript))
            ).Select(a => a.FullName ?? string.Empty)
            .Where(a => !string.IsNullOrWhiteSpace(a));
    }
}
