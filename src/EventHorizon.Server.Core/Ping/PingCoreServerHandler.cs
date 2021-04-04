namespace EventHorizon.Server.Core.Ping
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Server.Core.Connection;
    using EventHorizon.Server.Core.Events.Check;
    using EventHorizon.Server.Core.Events.Ping;
    using EventHorizon.Server.Core.Events.Register;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class PingCoreServerHandler : INotificationHandler<PingCoreServer>
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
            PingCoreServer notification,
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
                await connection.Api.Ping();
                _logger.LogWarning(
                    "Zone Core Server Ping."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Zone Core Server Ping failed"
                );
                // TODO: Publish Server Connection Check Failed
                // Have a service pick this up and try to reconnect to core server is not started.
                await _mediator.Publish(
                    new CheckCoreServerConnection() // This will check to make sure that the server is connected
                );
            }
        }
    }
}