namespace EventHorizon.Server.Core.Disconnected
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Server.Core.Connection.Disconnected;
    using EventHorizon.Zone.Core.Model.ServerProperty;
    using MediatR;

    public class CoreServerConnectionDisconnectedHandler : INotificationHandler<ServerCoreConnectionDisconnected>
    {
        private readonly IServerProperty _serverProperty;

        public CoreServerConnectionDisconnectedHandler(
            IServerProperty serverProperty
        )
        {
            _serverProperty = serverProperty;
        }
        
        public Task Handle(
            ServerCoreConnectionDisconnected notification,
            CancellationToken cancellationToken
        )
        {
            _serverProperty.Set(
                ServerPropertyKeys.SERVER_ID,
                null
            );
            return Task.CompletedTask;
        }
    }
}