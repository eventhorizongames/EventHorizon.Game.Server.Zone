namespace EventHorizon.Zone.System.Combat.Handlers.Level
{
    using EventHorizon.Zone.System.Combat.Events.Level;
    using EventHorizon.Zone.System.Combat.Model.Level;
    using EventHorizon.Zone.System.Combat.State;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class IncreaseHealthPointsLevelHandler
        : INotificationHandler<IncreaseHealthPointsLevelEvent>
    {
        readonly IEntityQueue<EntityLevelUp> _entityQueue;
        public IncreaseHealthPointsLevelHandler(
            IEntityQueue<EntityLevelUp> entityQueue
        )
        {
            _entityQueue = entityQueue;
        }

        public async Task Handle(IncreaseHealthPointsLevelEvent notification, CancellationToken cancellationToken)
        {
            await _entityQueue.Enqueue(new EntityLevelUp
            {
                EntityId = notification.EntityId,
                Property = LevelProperty.HP,
            });
        }
    }
}
