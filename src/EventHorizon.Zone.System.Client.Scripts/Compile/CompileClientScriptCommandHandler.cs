namespace EventHorizon.Zone.System.Client.Scripts.Compile
{
    using EventHorizon.Zone.Core.Events.SubProcess;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Client.Scripts.Actions.Reload;
    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Model.Client;
    using EventHorizon.Zone.System.Client.Scripts.Model.Generated;
    using EventHorizon.Zone.System.Client.Scripts.Validation;

    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class CompileClientScriptCommandHandler
        : IRequestHandler<CompileClientScriptCommand, StandardCommandResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly IJsonFileLoader _jsonFileLoader;
        private readonly ClientScriptsSettings _clientScriptsSettings;
        private readonly ClientScriptsState _state;

        public CompileClientScriptCommandHandler(
            ILogger<CompileClientScriptCommandHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader jsonFileLoader,
            ClientScriptsSettings clientScriptsSettings,
            ClientScriptsState state
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
            _jsonFileLoader = jsonFileLoader;
            _clientScriptsSettings = clientScriptsSettings;
            _state = state;
        }

        public async Task<StandardCommandResult> Handle(
            CompileClientScriptCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!string.IsNullOrEmpty(
                _state.Hash
            ))
            {
                // Check if scripts need to be compiled
                var hashResult = await _mediator.Send(
                    new NeedToCompileClientScripts(),
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

            // Start Sub Process - Client Scripts
            var processFullName = Path.Combine(
                _clientScriptsSettings.CompilerSubProcessDirectory,
                _clientScriptsSettings.CompilerSubProcess
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
                    ClientScriptsErrorCodes.CLIENT_SCRIPT_INVALID_PROCESS_ERROR_CODE
                );
            }

            // Load Generated Script Result
            // This will include a Success/Fail and ErrorCode with the Hash/ScriptAssembly filled on Success.
            var compiledResult = await _jsonFileLoader.GetFile<GeneratedClientScriptsResultModel>(
                Path.Combine(
                    _serverInfo.GeneratedPath,
                    GeneratedClientScriptsResultModel.GENERATED_FILE_NAME
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

            // Set Assembly State for Client Scripts
            _state.SetAssembly(
                compiledResult.Hash,
                compiledResult.ScriptAssembly
            );

            // Publish ClientAction Event 
            await _mediator.Publish(
                ClientScriptsAssemblyChangedClientActionToAllEvent.Create(
                    new ClientScriptsAssemblyChangedClientActionData(
                        compiledResult.Hash
                    )
                ),
                cancellationToken
            );

            return new();
        }
    }
}
