using EventHorizon.Zone.System.Agent.Model;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Register
{
    public struct RegisterAgentEvent : IRequest<AgentEntity>
    {
        public AgentEntity Agent { get; set; }
    }
}