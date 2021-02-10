namespace EventHorizon.Zone.System.Server.Scripts.Complie
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Events.SubProcess;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.Model.Generated;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using EventHorizon.Zone.System.Server.Scripts.Validation;
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

    public class CompileServerScriptsFromSubProcessCommandHandler
        : IRequestHandler<CompileServerScriptsFromSubProcessCommand, StandardCommandResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly IJsonFileLoader _jsonFileLoader;
        private readonly ServerScriptsSettings _scriptsSettings;
        private readonly ServerScriptsState _state;
        private readonly ServerScriptDetailsRepository _detailsRepository;
        private readonly ServerScriptRepository _scriptRepository;

        public CompileServerScriptsFromSubProcessCommandHandler(
            ILogger<CompileServerScriptsFromSubProcessCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader jsonFileLoader,
            ServerScriptsSettings scriptsSettings,
            ServerScriptsState state,
            ServerScriptDetailsRepository detailsRepository,
            ServerScriptRepository scriptRepository
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
            _jsonFileLoader = jsonFileLoader;
            _scriptsSettings = scriptsSettings;
            _state = state;
            _detailsRepository = detailsRepository;
            _scriptRepository = scriptRepository;
        }

        public async Task<StandardCommandResult> Handle(
            CompileServerScriptsFromSubProcessCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (!string.IsNullOrEmpty(
                    _state.CurrentHash
                ))
                {
                    // Check if scripts need to be compiled
                    var hashResult = await _mediator.Send(
                        new NeedToCompileServerScripts(),
                        cancellationToken
                    );
                    if (!hashResult.Success)
                    {
                        return new(
                            hashResult.ErrorCode
                        );
                    }

                    if (!hashResult.Result)
                    {
                        // No need to compile scripts
                        return new();
                    }
                }

                // Run Server Scripts Compile SubProcess
                var processFullName = Path.Combine(
                    _scriptsSettings.CompilerSubProcessDirectory,
                    _scriptsSettings.CompilerSubProcess
                );
                var processResult = await _mediator.Send(
                    new StartSubProcessCommand(
                        processFullName
                    ),
                    cancellationToken
                );
                if (!processResult.Success)
                {
                    _logger.LogError(
                        "Failed to Startup SubProcess to Compile Client Scripts: {ErrorCode} | {ProcessFullName}",
                        processResult.ErrorCode,
                        processFullName
                    );
                    return new(
                        processResult.ErrorCode
                    );
                }
                var process = processResult.Result;
                await process.WaitForExitAsync(
                    cancellationToken
                );
                if (process.ExitCode != 0)
                {
                    _logger.LogError(
                        "Bad ExitCode for SubProcess Client Scripts Compile. Check Logs for more Detail."
                    );
                    return new(
                        ServerScriptsErrorCodes.SERVER_SCRIPT_INVALID_PROCESS_ERROR_CODE
                    );
                }

                // Load Generated Script Result
                // This will include a Success/Fail and ErrorCode with the Hash filled on Success.
                var compiledResult = await _jsonFileLoader.GetFile<GeneratedServerScriptsResultModel>(
                    Path.Combine(
                        _serverInfo.GeneratedPath,
                        GeneratedServerScriptsResultModel.SCRIPTS_RESULT_FILE_NAME
                    )
                );
                if (!compiledResult.Success)
                {
                    _logger.LogError(
                        "Failed to Compile Client Scripts: {ErrorCode}",
                        compiledResult.ErrorCode
                    );
                    return new(
                        compiledResult.ErrorCode
                    );
                }

                // TODO: Move this into a Separate Command
                // TODO: This should use the ReadAllTextAsBytesFromFile or create new ReadFromFileAsFileStream
                var scriptsAssemblyStream = File.OpenRead(
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

                // TODO: Move this to after the scripts have been created
                if (scriptTypeList.Any())
                {
                    // Only clear current script repository if scripts were found.
                    _scriptRepository.Clear();
                }

                foreach (var item in scriptTypeList)
                {
                    var scriptInstance = assembly.CreateInstance(
                        item.FullName
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
                        _detailsRepository.Add(
                            id,
                            updatedDetails
                        );

                        // Add Script to Repository
                        _scriptRepository.Add(
                            serverScript
                        );
                    }
                }


                // Update ServerScriptState hash
                _state.UpdateHash(
                    compiledResult.Hash
                );


                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to Compile Server Scripts"
                );
                return new(
                    ServerScriptsErrorCodes.SERVER_SCRIPTS_FAILED_TO_COMPILE
                );
            }
        }

        private static IEnumerable<Type> GetScriptTypesList(
            Assembly assembly
        )
        {
            return assembly
                .GetTypes()
                .Where(
                    a => (a.FullName?.StartsWith("css_root+") ?? false)
                        && a.IsAssignableTo(typeof(ServerScript))
                );
        }
    }
}
