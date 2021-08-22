namespace EventHorizon.Zone.Core.ServerAction.Run
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.ServerAction.State;

    using MediatR;

    public class RunPendingServerActionsHandler : INotificationHandler<RunPendingServerActionsEvent>
    {
        readonly IMediator _mediator;
        readonly IServerActionQueue _serverActionQueue;
        public RunPendingServerActionsHandler(
            IMediator mediator,
            IServerActionQueue serverActionQueue
        )
        {
            _mediator = mediator;
            _serverActionQueue = serverActionQueue;
        }
        public async Task Handle(
            RunPendingServerActionsEvent notification,
            CancellationToken cancellationToken
        )
        {
            var list = _serverActionQueue.Take(
                10
            );
            foreach (var action in list.Values())
            {
                await _mediator.Publish(
                    action.EventToSend
                );
            }
        }
    }
}
