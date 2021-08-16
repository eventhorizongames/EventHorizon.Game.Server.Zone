namespace EventHorizon.Zone.System.Combat.Handlers.Level
{
    using EventHorizon.Zone.Core.Events.Entity.Client;
    using EventHorizon.Zone.Core.Events.Entity.Find;
    using EventHorizon.Zone.Core.Model.Entity.Client;
    using EventHorizon.Zone.System.Combat.Events.Level;
    using EventHorizon.Zone.System.Combat.Level;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class RunLevelUpForEntityHandler : INotificationHandler<RunLevelUpForEntityEvent>
    {
        readonly IMediator _mediator;
        readonly ILevelStateUpgrade _levelStateUpgrade;
        public RunLevelUpForEntityHandler(
            IMediator mediator,
            ILevelStateUpgrade levelStateUpgrade
        )
        {
            _mediator = mediator;
            _levelStateUpgrade = levelStateUpgrade;
        }
        public async Task Handle(RunLevelUpForEntityEvent notification, CancellationToken cancellationToken)
        {
            // Get Entity
            var entity = await _mediator.Send(
                new GetEntityByIdEvent
                {
                    EntityId = notification.EntityId
                }
            );
            if (entity.IsFound())
            {
                // Ignore request
                return;
            }
            var response = _levelStateUpgrade.Upgrade(
                entity,
                notification.Property
            );
            if (response.Success)
            {
                await _mediator.Publish(
                    new LevelUpSuccessfulEvent
                    {
                        EntityId = response.ChangedEntity.Id
                    }
                );
                await _mediator.Publish(
                    ClientActionEntityClientChangedToAllEvent.Create(
                        new EntityChangedData(
                            response.ChangedEntity
                        )
                    )
                );
            }
            else
            {
                await _mediator.Publish(
                    new LevelUpFailedEvent
                    {
                        EntityId = response.ChangedEntity.Id
                    }
                );
            }
        }
    }
}
