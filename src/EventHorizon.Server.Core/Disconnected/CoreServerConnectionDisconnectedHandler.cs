namespace EventHorizon.Server.Core.Disconnected
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Server.Core.Connection.Disconnected;
    using EventHorizon.Zone.Core.Model.ServerProperty;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class CoreServerConnectionDisconnectedHandler
        : INotificationHandler<ServerCoreConnectionDisconnected>
    {
        private readonly ILogger _logger;
        private readonly IServerProperty _serverProperty;

        public CoreServerConnectionDisconnectedHandler(
            ILogger<CoreServerConnectionDisconnectedHandler> logger,
            IServerProperty serverProperty
        )
        {
            _logger = logger;
            _serverProperty = serverProperty;
        }

        public Task Handle(
            ServerCoreConnectionDisconnected notification,
            CancellationToken cancellationToken
        )
        {
            _logger.LogWarning(
                "Disconnected from Core Server."
            );
            _serverProperty.Set(
                ServerPropertyKeys.SERVER_ID,
                null
            );
            return Task.CompletedTask;
        }
    }
}