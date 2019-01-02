using EventHorizon.Game.Server.Zone.Agent.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Get
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