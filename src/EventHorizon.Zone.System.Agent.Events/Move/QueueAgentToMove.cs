namespace EventHorizon.Zone.System.Agent.Events.Move
{
    using global::System.Collections.Generic;
    using global::System.Numerics;
    using MediatR;

    public struct QueueAgentToMove : IRequest
    {
        public long EntityId { get; }
        public Queue<Vector3> Path { get; }
        public Vector3 MoveTo { get; }

        public QueueAgentToMove(
            long entityId,
            Queue<Vector3> path,
            Vector3 moveTo
        )
        {
            EntityId = entityId;
            Path = path;
            MoveTo = moveTo;
        }
    }
}