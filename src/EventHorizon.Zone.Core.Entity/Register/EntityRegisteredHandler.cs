using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Events.Client.Actions;
using MediatR;
using EventHorizon.Zone.Core.Events.Entity.Register;

namespace EventHorizon.Zone.Core.Entity.Register
{
    public class EntityRegisteredHandler : INotificationHandler<EntityRegisteredEvent>
    {
        readonly IMediator _mediator;
        public EntityRegisteredHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            EntityRegisteredEvent notification,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Publish(
                new ClientActionEntityRegisteredToAllEvent
                {
                    Data = new EntityRegisteredData
                    {
                        Entity = notification.Entity,
                    }
                }
            );
        }
    }
}