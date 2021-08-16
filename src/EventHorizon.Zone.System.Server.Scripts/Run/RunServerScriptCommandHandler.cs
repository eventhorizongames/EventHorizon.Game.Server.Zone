namespace EventHorizon.Zone.System.Server.Scripts.Run
{
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Events.Run;
    using EventHorizon.Zone.System.Server.Scripts.Exceptions;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.Run.Model;
    using EventHorizon.Zone.System.Server.Scripts.System;

    using global::System;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class RunServerScriptCommandHandler
        : IRequestHandler<RunServerScriptCommand, ServerScriptResponse>
    {
        readonly ILogger _logger;
        readonly ServerScriptRepository _serverScriptRepository;
        readonly ServerScriptServices _serverScriptServices;

        public RunServerScriptCommandHandler(
            ILogger<RunServerScriptCommandHandler> logger,
            ServerScriptRepository serverScriptRepository,
            ServerScriptServices serverScriptServices
        )
        {
            _logger = logger;
            _serverScriptRepository = serverScriptRepository;
            _serverScriptServices = serverScriptServices;
        }

        public Task<ServerScriptResponse> Handle(
            RunServerScriptCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                _logger.LogDebug(
                    "Running Script: \n\r\t {ScriptId}",
                    request.Id
                );
                return _serverScriptRepository.Find(
                    request.Id
                ).Run(
                    _serverScriptServices,
                    new StandardServerScriptData(
                        request.Data
                    )
                );
            }
            catch (ServerScriptNotFound ex)
            {
                _logger.LogError(
                    ex,
                    "Server Script was Not Found. ScriptId: {ScriptId} Request: {@Request}",
                    request.Id,
                    request
                );
                return Task.FromResult(
                    new SystemServerScriptResponse(
                        false,
                        "server_script_not_found"
                    ) as ServerScriptResponse
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "General Server Script Exception. ScriptId: {ScriptId} Request: {@Request}",
                    request.Id,
                    request
                );
                return Task.FromResult(
                    new SystemServerScriptResponse(
                        false,
                        "general_server_script_error"
                    ) as ServerScriptResponse
                );
            }
        }
    }
}
