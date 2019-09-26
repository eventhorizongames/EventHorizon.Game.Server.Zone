using MediatR;

namespace EventHorizon.Zone.System.Agent.Events.Move
{
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
}