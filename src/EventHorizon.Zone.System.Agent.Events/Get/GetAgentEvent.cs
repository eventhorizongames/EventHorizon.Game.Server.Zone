using EventHorizon.Zone.System.Agent.Model;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Events.Get
{
    public struct GetAgentEvent : IRequest<AgentEntity>
    {
        public long EntityId { get; set; }
    }
}