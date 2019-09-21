using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Plugin.Zone.System.Combat.Events.Level;
using EventHorizon.Plugin.Zone.System.Combat.Level;
using MediatR;
using EventHorizon.Zone.Core.Model.Client.DataType;

namespace EventHorizon.Plugin.Zone.System.Combat.Handlers.Level
{
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
                    new ClientActionEntityClientChangedToAllEvent
                    {
                        Data = new EntityChangedData(
                            response.ChangedEntity
                        )
                    }
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