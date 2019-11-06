using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Register;
using EventHorizon.Game.Server.Zone.Server.Core.Check;
using EventHorizon.Game.Server.Zone.Server.Core.Register;
using EventHorizon.Game.Server.Zone.Server.Core.Stop;
using EventHorizon.Server.Core.External.Connection;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Server.Core.Ping
{
    public class PingCoreServerHandler : INotificationHandler<PingCoreServerEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly CoreServerConnectionFactory _connectionFactory;

        public PingCoreServerHandler(
            ILogger<PingCoreServerHandler> logger,
            IMediator mediator,
            CoreServerConnectionFactory connectionFactory
        )
        {
            _logger = logger;
            _mediator = mediator;
            _connectionFactory = connectionFactory;
        }

        public async Task Handle(
            PingCoreServerEvent notification,
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (!await _mediator.Send(
                    new QueryForRegistrationWithCoreServer()
                ))
                {
                    return;
                }

                var connection = await _connectionFactory.GetConnection();
                await connection.Ping();
                _logger.LogWarning(
                    "Zone Core Server Ping."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Zone Core Server Ping failed",
                    ex
                );
                // TODO: Publish Server Connection Check Failed
                // Have a service pick this up and try to reconnect to core server is not started.
                await _mediator.Publish(
                    new CheckCoreServerConnectionEvent() // This will check to make sure that the server is connected
                );
            }
        }
    }
}