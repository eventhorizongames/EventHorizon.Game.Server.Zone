namespace EventHorizon.Zone.System.Agent.Plugin.Move.Finished;

using EventHorizon.Zone.Core.Events.Entity.Client;
using EventHorizon.Zone.Core.Model.Entity.Client;
using EventHorizon.Zone.System.Agent.Plugin.Move.Events;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class AgentFinishedMoveEventHandler : INotificationHandler<AgentFinishedMoveEvent>
{
    private readonly IMediator _mediator;

    public AgentFinishedMoveEventHandler(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public Task Handle(
        AgentFinishedMoveEvent notification,
        CancellationToken cancellationToken
    )
    {
        return _mediator.Publish(
            ClientActionClientEntityStoppingToAllEvent.Create(
                new EntityClientStoppingData
                {
                    EntityId = notification.EntityId,
                }
            )
        );
    }
}
