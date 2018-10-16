

using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Plugin.Zone.System.Combat.Events.Level;
using EventHorizon.Plugin.Zone.System.Combat.Model.Level;
using EventHorizon.Plugin.Zone.System.Combat.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Handlers.Level
{
    public class IncreaseActionPointsLevelHandler : INotificationHandler<IncreaseActionPointsLevelEvent>
    {
        readonly IEntityQueue<EntityLevelUp> _entityQueue;
        public IncreaseActionPointsLevelHandler(
            IEntityQueue<EntityLevelUp> entityQueue
        )
        {
            _entityQueue = entityQueue;
        }

        public async Task Handle(IncreaseActionPointsLevelEvent notification, CancellationToken cancellationToken)
        {
            await _entityQueue.Enqueue(new EntityLevelUp
            {
                EntityId = notification.EntityId,
                Property = LevelProperty.AP,
            });
        }
    }
}