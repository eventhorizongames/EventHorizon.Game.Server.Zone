using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.System.Combat.Events.Life;
using EventHorizon.Zone.System.Combat.Life;

using MediatR;

namespace EventHorizon.Zone.System.Combat.Handlers.Life
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
                    new LifeStateChangedEvent
                    {
                        EntityId = response.ChangedEntity.Id
                    }
                );
            }
        }
    }
}
