using EventHorizon.Game.Server.Zone.Agent.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.PopulateData
{
    public struct PopulateAgentEntityDataEvent : IRequest<AgentEntity>
    {
        public AgentEntity Agent { get; set; }
    }
}