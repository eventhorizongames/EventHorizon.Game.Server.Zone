namespace EventHorizon.Zone.System.Agent.Events.Move;

using MediatR;

public struct IsAgentMoving : IRequest<bool>
{
    public long EntityId { get; }

    public IsAgentMoving(
        long entityId
    )
    {
        EntityId = entityId;
    }
}
