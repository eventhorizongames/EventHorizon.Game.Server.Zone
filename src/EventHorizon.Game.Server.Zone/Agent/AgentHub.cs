using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Zone.System.Agent
{
    // TODO: Check to make sure this is not used anymore.
    // If it is still used move it into Editor, I think that is where is this used.
    // If not used just delete it.
    [Authorize]
    public class AgentHub : Hub
    {
        readonly IMediator _mediator;
        public AgentHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override Task OnConnectedAsync()
        {
            if (!Context.User.IsInRole("Admin"))
            {
                throw new Exception("no_role");
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<AgentEntity>> GetAgentList()
        {
            return await _mediator.Send(
                new GetAgentListEvent()
            );
        }
    }
}