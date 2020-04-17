namespace EventHorizon.Zone.System.Agent.Plugin.Move.Events
{
    using global::System.Numerics;
    using MediatR;

    public struct MoveAgentToPositionEvent : IRequest
    {
        public long AgentId { get; }
        public Vector3 ToPosition { get; }

        public MoveAgentToPositionEvent(
            long agentId,
            Vector3 toPosition
        )
        {
            AgentId = agentId;
            ToPosition = toPosition;
        }
    }
}