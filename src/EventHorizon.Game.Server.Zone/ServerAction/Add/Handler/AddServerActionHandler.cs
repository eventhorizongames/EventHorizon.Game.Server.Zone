using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.ServerAction;
using EventHorizon.Game.Server.Zone.ServerAction.Model;
using EventHorizon.Game.Server.Zone.ServerAction.State;
using MediatR;

namespace EventHorizon.Game.Server.Zone.ServerAction.Add.Handler
{
    public class AddServerActionHandler : INotificationHandler<AddServerActionEvent>
    {
        readonly IServerActionQueue _serverActionQueue;
        public AddServerActionHandler(IServerActionQueue serverActionQueue)
        {
            _serverActionQueue = serverActionQueue;
        }

        public async Task Handle(AddServerActionEvent notification, CancellationToken cancellationToken)
        {
            await _serverActionQueue.Push(new ServerActionEntity(notification.RunAt, notification.EventToSend));
        }
    }
}