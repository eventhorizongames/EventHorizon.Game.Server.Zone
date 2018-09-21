using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Player;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Entity.Registered.Handler
{
    public class EntityRegisteredHandler : INotificationHandler<EntityRegisteredEvent>
    {
        readonly IMediator _mediator;
        public EntityRegisteredHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(EntityRegisteredEvent notification, CancellationToken cancellationToken)
        {
            await _mediator.Publish(new ClientActionEvent
            {
                Action = "EntityRegistered",
                Data = new EntityRegisteredData
                {
                    Entity = notification.Entity,
                }
            });
        }
    }
}