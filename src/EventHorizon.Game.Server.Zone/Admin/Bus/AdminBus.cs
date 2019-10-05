using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command.Model;
using EventHorizon.Game.Server.Zone.Info.Query;
using EventHorizon.Zone.Core.Events.Admin.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Admin.Bus
{
    [Authorize]
    public class AdminBus : Hub
    {
        readonly IMediator _mediator;
        public AdminBus(
            IMediator mediator
        )
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

        public Task Command(
            string command,
            object data
        )
        {
            return _mediator.Publish(
                new AdminCommandEvent(
                    Context.ConnectionId,
                    AdminCommandFromString.CreateFromString(
                        command
                    ),
                    data
                )
            );
        }

        public Task<IDictionary<string, object>> ZoneInfo()
        {
            return _mediator.Send(
                new QueryForFullZoneInfo()
            );
        }
    }
}