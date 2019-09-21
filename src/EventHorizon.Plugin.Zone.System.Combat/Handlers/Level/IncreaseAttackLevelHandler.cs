


using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Combat.Events.Level;
using EventHorizon.Zone.System.Combat.Model.Level;
using EventHorizon.Plugin.Zone.System.Combat.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Handlers.Level
{
    public class IncreaseAttackLevelHandler : INotificationHandler<IncreaseAttackLevelEvent>
    {
        readonly IEntityQueue<EntityLevelUp> _entityQueue;
        public IncreaseAttackLevelHandler(
            IEntityQueue<EntityLevelUp> entityQueue
        )
        {
            _entityQueue = entityQueue;
        }

        public async Task Handle(IncreaseAttackLevelEvent notification, CancellationToken cancellationToken)
        {
            await _entityQueue.Enqueue(new EntityLevelUp
            {
                EntityId = notification.EntityId,
                Property = LevelProperty.ATTACK,
            });
        }
    }
}