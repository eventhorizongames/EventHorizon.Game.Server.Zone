using System;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Player.Connected;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Player
{
    [Authorize]
    public class PlayerHub : Hub
    {
        readonly IMediator _mediator;
        public PlayerHub(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task OnConnectedAsync()
        {
            var playerConnectionId = Context.ConnectionId;
            var playerId = Context.User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value;

            await _mediator.Publish(new PlayerConnectedEvent
            {
                Id = playerId,
                ConnectionId = playerConnectionId,
            });
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

        }
    }
}