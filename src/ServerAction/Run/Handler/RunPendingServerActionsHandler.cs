using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.ServerAction.State;
using EventHorizon.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.ServerAction.Run.Handler
{
    public class RunPendingServerActionsHandler : INotificationHandler<RunPendingServerActionsEvent>
    {
        readonly IMediator _mediator;
        readonly IServerActionQueue _serverActionQueue;
        public RunPendingServerActionsHandler(IMediator mediator, IServerActionQueue serverActionQueue)
        {
            _mediator = mediator;
            _serverActionQueue = serverActionQueue;
        }
        public async Task Handle(RunPendingServerActionsEvent notification, CancellationToken cancellationToken)
        {
            var list = await _serverActionQueue.Take(10);
            foreach (var action in list.Values())
            {
                await _mediator.Publish(action.EventToSend);
            }
        }
    }
}