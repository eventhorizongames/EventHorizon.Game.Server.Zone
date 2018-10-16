using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Game.Server.Zone.Events.Entity.Find;
using EventHorizon.Plugin.Zone.System.Combat.Events.Life;
using EventHorizon.Plugin.Zone.System.Combat.Life;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Handlers.Life
{
    public class RunEntityLifeStateChangeHandler : INotificationHandler<RunEntityLifeStateChangeEvent>
    {
        readonly IMediator _mediator;
        readonly ILifeStateChange _lifeStateChange;
        public RunEntityLifeStateChangeHandler(
            IMediator mediator,
            ILifeStateChange lifeStateChange
        )
        {
            _mediator = mediator;
            _lifeStateChange = lifeStateChange;
        }

        public async Task Handle(RunEntityLifeStateChangeEvent notification, CancellationToken cancellationToken)
        {
            // Get Entity
            var entity = await _mediator.Send(
                new GetEntityByIdEvent
                {
                    EntityId = notification.EntityId
                }
            );
            if (!entity.IsFound())
            {
                // Ignore request
                return;
            }

            var response = _lifeStateChange.Change(
                entity,
                notification.Property,
                notification.Points
            );
            if (response.Success)
            {
                // Publish event Entity Life State Changed
                await _mediator.Publish(
                    new LifeStateUpdatedEvent
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