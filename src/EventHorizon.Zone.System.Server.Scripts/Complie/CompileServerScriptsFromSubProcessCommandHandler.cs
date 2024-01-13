namespace EventHorizon.Zone.System.Server.Scripts.Complie;

using EventHorizon.Zone.Core.Events.SubProcess;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Server.Scripts.Actions;
using EventHorizon.Zone.System.Server.Scripts.Api;
using EventHorizon.Zone.System.Server.Scripts.Load;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.Model.Generated;
using EventHorizon.Zone.System.Server.Scripts.Validation;

using global::System;
using global::System.IO;
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

    public CompileServerScriptsFromSubProcessCommandHandler(
        ILogger<CompileServerScriptsFromSubProcessCommandHandler> logger,
        IMediator mediator,
        ServerInfo serverInfo,
        IJsonFileLoader jsonFileLoader,
        ServerScriptsSettings scriptsSettings,
        ServerScriptsState state
    )
    {
        _logger = logger;
        _mediator = mediator;
        _serverInfo = serverInfo;
        _jsonFileLoader = jsonFileLoader;
        _scriptsSettings = scriptsSettings;
        _state = state;
    }

    public async Task<StandardCommandResult> Handle(
        CompileServerScriptsFromSubProcessCommand request,
        CancellationToken cancellationToken
    )
    {
        var compileWasDone = false;
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

            compileWasDone = true;
            await _mediator.Publish(
                ServerScriptsSystemCompilingScriptsClientActionToAllEvent.Create(),
                cancellationToken
            );

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
                    "Bad ExitCode for SubProcess Server Scripts Compile. Check Logs for more Detail."
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
            if (compiledResult == null
                || !compiledResult.Success
            )
            {
                var errorCode = compiledResult?.ErrorCode ?? ServerScriptsErrorCodes.SERVER_SCRIPTS_FAILED_TO_COMPILE;
                _logger.LogError(
                    "Failed to Compile Server Scripts: {ErrorCode}",
                    errorCode
                );

                _state.SetErrorCode(
                    errorCode
                );

                if (compiledResult?.ScriptErrorDetailsList != null)
                {
                    _state.SetErrorState(
                        compiledResult.ScriptErrorDetailsList
                    );
                }

                return new(
                    errorCode
                );
            }

            var loadAssemblyResult = await _mediator.Send(
                new LoadNewServerScriptAssemblyCommand(),
                cancellationToken
            );

            if (!loadAssemblyResult.Success)
            {
                return loadAssemblyResult;
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
        finally
        {
            if (compileWasDone)
            {
                await _mediator.Publish(
                    ServerScriptsSystemFinishedScriptsCompileClientActionToAllEvent.Create(),
                    cancellationToken
                );
            }
        }
    }
}
