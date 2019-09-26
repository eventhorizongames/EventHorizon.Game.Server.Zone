using EventHorizon.Zone.System.Agent.Model;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Events.Get
{
    public struct FindAgentByIdEvent : IRequest<AgentEntity>
    {
        public string AgentId { get; set; }

        public FindAgentByIdEvent(
            string agentId
        )
        {
            AgentId = agentId;
        }
    }
}