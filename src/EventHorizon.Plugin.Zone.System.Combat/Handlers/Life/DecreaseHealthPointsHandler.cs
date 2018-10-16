
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Plugin.Zone.System.Combat.Events.Life;
using EventHorizon.Plugin.Zone.System.Combat.Model.Life;
using EventHorizon.Plugin.Zone.System.Combat.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Handlers.Life
{
    public class DecreaseHealthPointsHandler : INotificationHandler<DecreaseHealthPointsEvent>
    {
        readonly IEntityQueue<UpdateEntityLife> _entityQueue;
        public DecreaseHealthPointsHandler(
            IEntityQueue<UpdateEntityLife> entityQueue
        )
        {
            _entityQueue = entityQueue;
        }

        public async Task Handle(DecreaseHealthPointsEvent notification, CancellationToken cancellationToken)
        {
            await _entityQueue.Enqueue(new UpdateEntityLife
            {
                EntityId = notification.EntityId,
                Property = LifeProperty.HP,
                // Make the points of the event negative (-)
                Points = -notification.Points,
            });
        }
    }
}