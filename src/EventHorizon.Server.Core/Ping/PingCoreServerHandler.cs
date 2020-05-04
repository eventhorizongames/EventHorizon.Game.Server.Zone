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
        private readonly CoreServerConnection _connection;

        public PingCoreServerHandler(
            ILogger<PingCoreServerHandler> logger,
            IMediator mediator,
            CoreServerConnection connection
        )
        {
            _logger = logger;
            _mediator = mediator;
            _connection = connection;
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

                await _connection.Api.Ping();
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
                    new CheckCoreServerConnection() // This will check to make sure that the server is connected
                );
            }
        }
    }
}