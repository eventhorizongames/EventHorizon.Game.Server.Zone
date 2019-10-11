using System;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Player.Events.Connected;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Zone.System.Player.ExternalHub
{
    [Authorize]
    public partial class PlayerHub : Hub
    {
        readonly IMediator _mediator;

        public PlayerHub(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        
        public override async Task OnConnectedAsync()
        {
            await _mediator.Publish(
                new PlayerConnectedEvent(
                    Context.User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value,
                    Context.ConnectionId
                )
            );
        }

        public override async Task OnDisconnectedAsync(
            Exception exception
        )
        {
            await _mediator.Publish(
                new PlayerDisconnectedEvent(
                    Context.User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value
                )
            );
        }
    }
}