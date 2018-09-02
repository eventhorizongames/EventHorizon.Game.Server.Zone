using EventHorizon.Game.Server.Zone.Agent.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Get
{
    public struct GetAgentEvent : IRequest<AgentEntity>
    {
        public long AgentId { get; set; }
    }
}