using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Events.Level;
using EventHorizon.Plugin.Zone.System.Combat.Model;
using EventHorizon.Plugin.Zone.System.Combat.Model.Level;
using EventHorizon.Plugin.Zone.System.Combat.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Handlers.Level
{
    public class IncreaseExperienceHandler : INotificationHandler<IncreaseExperienceEvent>
    {
        readonly IMediator _mediator;
        public IncreaseExperienceHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(IncreaseExperienceEvent notification, CancellationToken cancellationToken)
        {
            var entity = await _mediator.Send(
                new GetEntityByIdEvent
                {
                    EntityId = notification.EntityId
                }
            );
            if (!entity.IsFound())
            {
                // Ignore
                return;
            }

            var entityLevelState = entity.GetProperty<LevelState>(LevelState.PROPERTY_NAME);
            entityLevelState.Experience += notification.Points;
            entityLevelState.AllTimeExperience += notification.Points;
        }
    }
}