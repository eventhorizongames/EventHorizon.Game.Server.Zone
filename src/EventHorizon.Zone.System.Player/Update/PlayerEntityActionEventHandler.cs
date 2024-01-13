namespace EventHorizon.Zone.System.Player.Update;

using EventHorizon.Zone.Core.Events.Entity.Action;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.Events.Update;

using global::System.Diagnostics.CodeAnalysis;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class PlayerEntityActionEventHandler
    : INotificationHandler<EntityActionEvent>
{
    private readonly IMediator _mediator;

    public PlayerEntityActionEventHandler(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public async Task Handle(
        EntityActionEvent notification,
        CancellationToken cancellationToken
    )
    {
        if (!IsPlayerEntity(
            notification.Entity
        ))
        {
            return;
        }

        await _mediator.Publish(
            new PlayerGlobalUpdateEvent(
                notification.Entity.To<PlayerEntity>()
            ),
            cancellationToken
        );
    }

    private static bool IsPlayerEntity(
        [NotNullWhen(true)] IObjectEntity? entity
    ) => entity is not null
        && entity.Type == EntityType.PLAYER
        && entity is PlayerEntity;
}
