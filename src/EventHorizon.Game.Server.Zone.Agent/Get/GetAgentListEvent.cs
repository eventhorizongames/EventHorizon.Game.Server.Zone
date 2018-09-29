using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Agent.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Get
{
    public struct GetAgentListEvent : IRequest<IEnumerable<AgentEntity>>
    {
        
    }
}