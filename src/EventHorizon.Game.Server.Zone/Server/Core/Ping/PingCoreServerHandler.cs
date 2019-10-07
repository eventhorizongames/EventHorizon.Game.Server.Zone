using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Register;
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
                await _mediator.Publish(
                    new StopCoreServerConnectionEvent()
                );
                await _mediator.Publish(
                    new RegisterWithCoreServerEvent()
                );
            }
        }
    }
}