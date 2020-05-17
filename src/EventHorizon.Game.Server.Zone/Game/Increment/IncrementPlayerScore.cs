namespace EventHorizon.Game.Server.Zone.Game.Increment
{
    using MediatR;

    public struct IncrementPlayerScore : IRequest
    {
        public long PlayerEntityId { get; }

        public IncrementPlayerScore(
            long playerEntityId
        )
        {
            PlayerEntityId = playerEntityId;
        }
    }
}