namespace EventHorizon.Game.Capture.Logic
{
    using EventHorizon.Zone.Core.Model.Player;

    using MediatR;

    public struct RunEscapeOfCaptures : IRequest
    {
        public PlayerEntity PlayerEntity { get; }

        public RunEscapeOfCaptures(
            PlayerEntity playerEntity
        )
        {
            PlayerEntity = playerEntity;
        }
    }
}
