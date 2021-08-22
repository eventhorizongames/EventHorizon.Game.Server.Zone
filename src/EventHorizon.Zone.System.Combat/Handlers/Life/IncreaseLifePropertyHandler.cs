namespace EventHorizon.Zone.System.Combat.Handlers.Life
{
    using EventHorizon.Zone.System.Combat.Events.Life;
    using EventHorizon.Zone.System.Combat.Model.Life;
    using EventHorizon.Zone.System.Combat.State;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class IncreaseLifePropertyHandler
        : INotificationHandler<IncreaseLifePropertyEvent>
    {
        readonly IEntityQueue<ChangeEntityLife> _entityQueue;
        public IncreaseLifePropertyHandler(
            IEntityQueue<ChangeEntityLife> entityQueue
        )
        {
            _entityQueue = entityQueue;
        }

        public async Task Handle(IncreaseLifePropertyEvent notification, CancellationToken cancellationToken)
        {
            await _entityQueue.Enqueue(new ChangeEntityLife
            {
                EntityId = notification.EntityId,
                Property = LifeProperty.HP,
                Points = notification.Points,
            });
        }
    }
}
