namespace EventHorizon.Game.Server.Zone.Player.Move.Stop;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Entity.Client;
using EventHorizon.Zone.Core.Model.Entity.Client;

using MediatR;

public class StopPlayerEventHandler
    : INotificationHandler<StopPlayerEvent>
{
    private readonly IMediator _mediator;

    public StopPlayerEventHandler(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public Task Handle(
        StopPlayerEvent notification,
        CancellationToken cancellationToken
    )
    {
        return _mediator.Publish(
            ClientActionClientEntityStoppingToAllEvent.Create(
                new EntityClientStoppingData
                {
                    EntityId = notification.Player.Id
                }
            ),
            cancellationToken
        );
    }
}
