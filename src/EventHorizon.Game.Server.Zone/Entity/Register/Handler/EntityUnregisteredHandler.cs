using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Game.Server.Zone.Player;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Entity.Registered.Handler
{
    public class EntityUnregisteredHandler : INotificationHandler<EntityUnregisteredEvent>
    {
        readonly IMediator _mediator;
        public EntityUnregisteredHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(EntityUnregisteredEvent notification, CancellationToken cancellationToken)
        {
            await _mediator.Publish(new ClientActionEntityUnregisteredToAllEvent
            {
                Data = new EntityUnregisteredData
                {
                    EntityId = notification.EntityId,
                }
            });
        }
    }
}