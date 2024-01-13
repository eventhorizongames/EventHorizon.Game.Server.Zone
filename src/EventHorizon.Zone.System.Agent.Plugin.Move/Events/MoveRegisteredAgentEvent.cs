namespace EventHorizon.Zone.System.Agent.Plugin.Move.Events;

using MediatR;

public struct MoveRegisteredAgentEvent : INotification
{
    public long EntityId { get; }

    public MoveRegisteredAgentEvent(
        long entityId
    )
    {
        EntityId = entityId;
    }
}
