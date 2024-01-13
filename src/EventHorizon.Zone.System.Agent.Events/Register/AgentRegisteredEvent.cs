namespace EventHorizon.Zone.System.Agent.Events.Register;

using MediatR;

public struct AgentRegisteredEvent : INotification
{
    public string AgentId { get; }

    public AgentRegisteredEvent(
        string agentId
    )
    {
        AgentId = agentId;
    }
}
