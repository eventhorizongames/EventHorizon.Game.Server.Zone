namespace EventHorizon.Zone.System.Combat.Handlers.Level
{
    using EventHorizon.Zone.System.Combat.Events.Level;
    using EventHorizon.Zone.System.Combat.Model.Level;
    using EventHorizon.Zone.System.Combat.State;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class IncreaseAttackLevelHandler
        : INotificationHandler<IncreaseAttackLevelEvent>
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
