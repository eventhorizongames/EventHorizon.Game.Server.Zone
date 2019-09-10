using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;
using EventHorizon.Zone.System.Server.Scripts.Exceptions;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.System;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Server.Scripts.Run
{
    public struct RunServerScriptCommandHandler : IRequestHandler<RunServerScriptCommand, ServerScriptResponse>
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
                return _serverScriptRepository.Find(
                    request.Id
                ).Run(
                    _serverScriptServices,
                    request.Data
                );
            }
            catch (
                ServerScriptNotFound ex
            )
            {
                _logger.LogError(
                    ex,
                    "Server Script was Not Found",
                    request
                );
                return Task.FromResult(
                    new SystemServerScriptResponse(
                        false,
                        "server_script_not_found"
                    ) as ServerScriptResponse
                );
            }
            catch (
                Exception ex
            )
            {
                _logger.LogError(
                    ex,
                    "General Server Script Exception",
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