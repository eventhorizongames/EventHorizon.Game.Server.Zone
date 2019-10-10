using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Zone.System.Admin.ExternalHub
{
    [Authorize]
    public partial class AdminHub : Hub
    {
        readonly IMediator _mediator;

        public AdminHub(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public override Task OnConnectedAsync()
        {
            if (!Context.User.IsInRole(
                "Admin"
            ))
            {
                throw new Exception(
                    "no_role"
                );
            }
            return Task.CompletedTask;
        }
    }
}