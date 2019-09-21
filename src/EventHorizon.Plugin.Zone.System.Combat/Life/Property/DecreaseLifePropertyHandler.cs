using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Combat.Events.Life;
using EventHorizon.Plugin.Zone.System.Combat.Model.Life;
using EventHorizon.Plugin.Zone.System.Combat.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Life.Property
{
    public class DecreaseLifePropertyHandler : INotificationHandler<DecreaseLifePropertyEvent>
    {
        readonly IEntityQueue<ChangeEntityLife> _entityQueue;
        public DecreaseLifePropertyHandler(
            IEntityQueue<ChangeEntityLife> entityQueue
        )
        {
            _entityQueue = entityQueue;
        }
        public async Task Handle(DecreaseLifePropertyEvent notification, CancellationToken cancellationToken)
        {
            await _entityQueue.Enqueue(new ChangeEntityLife
            {
                EntityId = notification.EntityId,
                Property = notification.Property,
                Points = -notification.Points,
            });
        }
    }
}