using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Model;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Events.Get
{
    public struct GetAgentListEvent : IRequest<IEnumerable<AgentEntity>>
    {
        
    }
}