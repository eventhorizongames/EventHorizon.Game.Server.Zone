namespace EventHorizon.Zone.System.Agent.Events.Register;

using MediatR;

public struct AgentUnRegisteredEvent : INotification
{
    public long EntityId { get; }
    public string AgentId { get; }

    public AgentUnRegisteredEvent(
        long entityId,
        string agentId
    )
    {
        EntityId = entityId;
        AgentId = agentId;
    }
}
