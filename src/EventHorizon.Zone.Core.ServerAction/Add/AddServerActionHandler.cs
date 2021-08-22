namespace EventHorizon.Zone.Core.ServerAction.ServerAction.Add
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.ServerAction;
    using EventHorizon.Zone.Core.ServerAction.Model;
    using EventHorizon.Zone.Core.ServerAction.State;

    using MediatR;

    public class AddServerActionHandler : INotificationHandler<AddServerActionEvent>
    {
        readonly IServerActionQueue _serverActionQueue;
        public AddServerActionHandler(
            IServerActionQueue serverActionQueue
        )
        {
            _serverActionQueue = serverActionQueue;
        }

        public Task Handle(
            AddServerActionEvent notification,
            CancellationToken cancellationToken
        )
        {
            _serverActionQueue.Push(
                new ServerActionEntity(
                    notification.RunAt,
                    notification.EventToSend
                )
            );
            return Task.CompletedTask;
        }
    }
}
