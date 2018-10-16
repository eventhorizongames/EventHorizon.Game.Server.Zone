using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Plugin.Zone.System.Combat.Events.Life;
using EventHorizon.Plugin.Zone.System.Combat.Model.Life;
using EventHorizon.Plugin.Zone.System.Combat.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Handlers.Life
{
    public class IncreaseHealthPointsHandler : INotificationHandler<IncreaseHealthPointsEvent>
    {
        readonly IEntityQueue<ChangeEntityLife> _entityQueue;
        public IncreaseHealthPointsHandler(
            IEntityQueue<ChangeEntityLife> entityQueue
        )
        {
            _entityQueue = entityQueue;
        }

        public async Task Handle(IncreaseHealthPointsEvent notification, CancellationToken cancellationToken)
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