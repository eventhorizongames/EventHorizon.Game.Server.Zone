using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Client.DataType;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Move.Stop
{
    public struct StopPlayerEventHandler : INotificationHandler<StopPlayerEvent>
    {
        readonly IMediator _mediator;
        
        public StopPlayerEventHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public Task Handle(
            StopPlayerEvent notification,
            CancellationToken cancellationToken
        )
        {
            return _mediator.Publish(
                new ClientActionClientEntityStoppingToAllEvent
                {
                    Data = new EntityClientStoppingData
                    {
                        EntityId = notification.Player.Id
                    },
                }
            );
        }
    }
}