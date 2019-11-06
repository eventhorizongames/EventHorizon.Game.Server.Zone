using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Server.Core.External.Events.Connection;
using EventHorizon.Zone.Core.Model.ServerProperty;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Server.Core.Disconnected
{
    public class CoreServerConnectionDisconnectedEventHandler : INotificationHandler<ServerCoreConnectionDisconnectedEvent>
    {
        private readonly IServerProperty _serverProperty;

        public CoreServerConnectionDisconnectedEventHandler(
            IServerProperty serverProperty
        )
        {
            _serverProperty = serverProperty;
        }
        public Task Handle(
            ServerCoreConnectionDisconnectedEvent notification,
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