using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Entity.Find;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Events;
using EventHorizon.Plugin.Zone.System.Combat.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Handlers
{
    public class LowerEntityHealthHandler : INotificationHandler<LowerEntityHealthEvent>
    {
        readonly IMediator _mediator;
        public LowerEntityHealthHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Handle(LowerEntityHealthEvent notification, CancellationToken cancellationToken)
        {
            var entity = await _mediator.Send(new GetEntityByIdEvent
            {
                EntityId = notification.EntityId
            });

            if (!entity.IsFound())
            {
                return;
            }

            var lifeState = entity.GetProperty<LifeState>(LifeState.PROPERTY_NAME);
        }
    }
}