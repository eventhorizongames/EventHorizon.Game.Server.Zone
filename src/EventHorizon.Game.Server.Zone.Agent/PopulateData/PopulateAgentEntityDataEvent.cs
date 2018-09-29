using EventHorizon.Game.Server.Zone.Agent.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.PopulateData
{
    public struct PopulateAgentEntityDataEvent : INotification
    {
        public AgentEntity Agent { get; set; }
    }
}