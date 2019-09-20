using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Player;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Actions.StopPlayer
{
    public struct StopPlayer : IRequest
    {
        public PlayerEntity Player { get; }
        public StopPlayer(
            PlayerEntity player
        )
        {
            Player = player;
        }
        public struct StopPlayerHandler : IRequestHandler<StopPlayer>
        {
            readonly IMediator _mediator;
            public StopPlayerHandler(
                IMediator mediator
            )
            {
                _mediator = mediator;
            }
            public async Task<Unit> Handle(StopPlayer request, CancellationToken cancellationToken)
            {
                await _mediator.Publish(new ClientActionClientEntityStoppingToAllEvent
                {
                    Data = new EntityClientStoppingData
                    {
                        EntityId = request.Player.Id
                    },
                });
                return Unit.Value;
            }
        }
    }
}