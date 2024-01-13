namespace EventHorizon.Zone.System.Agent.Events.PopulateData;

using EventHorizon.Zone.System.Agent.Model;

using MediatR;

public struct PopulateAgentEntityDataEvent : INotification
{
    public AgentEntity Agent { get; set; }
}
