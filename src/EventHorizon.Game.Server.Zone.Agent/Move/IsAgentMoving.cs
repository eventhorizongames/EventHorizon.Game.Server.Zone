using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move
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