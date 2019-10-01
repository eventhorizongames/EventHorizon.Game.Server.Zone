using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Connection;
using EventHorizon.Game.Server.Zone.Core.Connection.Stop;
using EventHorizon.Game.Server.Zone.Core.Register;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Core.Ping.Handler
{
    public class PingCoreServerHandler : INotificationHandler<PingCoreServerEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ICoreConnectionFactory _connectionFactory;

        public PingCoreServerHandler(ILogger<PingCoreServerHandler> logger,
            IMediator mediator,
            ICoreConnectionFactory connectionFactory)
        {
            _logger = logger;
            _mediator = mediator;
            _connectionFactory = connectionFactory;
        }

        public async Task Handle(PingCoreServerEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var connection = await _connectionFactory.GetConnection();
                await connection.Ping();
                _logger.LogWarning("Zone Core Server Ping.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Zone Core Server Ping failed", ex);
                await _mediator.Publish(new StopZoneCoreConnectionEvent());
                await _mediator.Publish(new RegisterWithCoreServerEvent());
            }
        }
    }
}