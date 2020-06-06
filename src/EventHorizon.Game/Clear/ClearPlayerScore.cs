namespace EventHorizon.Game.Clear
{
    using MediatR;

    public struct ClearPlayerScore : IRequest
    {
        public long EntityId { get; }

        public ClearPlayerScore(
            long entityId
        )
        {
            EntityId = entityId;
        }
    }
}
