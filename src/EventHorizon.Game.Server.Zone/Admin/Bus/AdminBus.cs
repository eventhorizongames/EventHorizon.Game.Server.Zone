using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command;
using EventHorizon.Game.Server.Zone.Admin.Command.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Admin.Bus
{
    [Authorize]
    public class AdminBus : Hub
    {
        readonly IMediator _mediator;
        public AdminBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override Task OnConnectedAsync()
        {
            if (!Context.User.IsInRole("Admin"))
            {
                throw new System.Exception("no_role");
            }
            return Task.CompletedTask;
        }

        public async Task<AdminCommandResponse> Command(string command, object data)
        {
            return await _mediator.Send(new AdminCommandEvent
            {
                Command = command,
                Data = data,
            });
        }
    }
}