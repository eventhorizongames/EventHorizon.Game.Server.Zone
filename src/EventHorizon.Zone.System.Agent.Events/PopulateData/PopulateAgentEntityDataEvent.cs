using EventHorizon.Zone.System.Agent.Model;

using MediatR;

namespace EventHorizon.Zone.System.Agent.Events.PopulateData
{
    public struct PopulateAgentEntityDataEvent : INotification
    {
        public AgentEntity Agent { get; set; }
    }
}
