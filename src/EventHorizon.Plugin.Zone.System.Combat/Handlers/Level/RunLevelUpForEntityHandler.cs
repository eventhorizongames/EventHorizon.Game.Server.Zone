using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Game.Server.Zone.Events.Entity.Find;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Events.Level;
using EventHorizon.Plugin.Zone.System.Combat.Level;
using EventHorizon.Plugin.Zone.System.Combat.Model;
using EventHorizon.Plugin.Zone.System.Combat.Model.Level;
using MediatR;

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
                    new LevelStateUpdatedEvent
                    {
                        EntityId = response.ChangedEntity.Id
                    }
                );
                await _mediator.Publish(
                    new ClientActionEntityClientChangedEvent
                    {
                        Data = new EntityChangedData
                        {
                            Details = response.ChangedEntity
                        }
                    }
                );
            }
        }
    }
}