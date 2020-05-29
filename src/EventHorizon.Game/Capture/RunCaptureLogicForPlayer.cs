namespace EventHorizon.Game.Capture
{
    using MediatR;

    public struct RunCaptureLogicForPlayer : IRequest
    {
        public long PlayerEntityId { get; }

        public RunCaptureLogicForPlayer(
            long playerEntityId
        )
        {
            PlayerEntityId = playerEntityId;
        }
    }
}
