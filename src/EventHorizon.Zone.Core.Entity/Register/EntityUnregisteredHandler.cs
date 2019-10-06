using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Events.Client.Actions;
using MediatR;
using EventHorizon.Zone.Core.Events.Entity.Register;

namespace EventHorizon.Zone.Core.Entity.Register
{
    public class EntityUnregisteredHandler : INotificationHandler<EntityUnRegisteredEvent>
    {
        readonly IMediator _mediator;
        public EntityUnregisteredHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            EntityUnRegisteredEvent notification, 
            CancellationToken cancellationToken
        )
        {
            await _mediator.Publish(
                new ClientActionEntityUnregisteredToAllEvent
                {
                    Data = new EntityUnregisteredData
                    {
                        EntityId = notification.EntityId,
                    }
                }
            );
        }
    }
}