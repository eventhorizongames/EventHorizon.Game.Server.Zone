namespace EventHorizon.Game.Capture.Logic
{
    using EventHorizon.Zone.Core.Model.Player;
    using MediatR;

    public struct ProcessFiveSecondCaptureLogic : IRequest
    {
        public PlayerEntity PlayerEntity { get; }

        public ProcessFiveSecondCaptureLogic(
            PlayerEntity playerEntity
        )
        {
            PlayerEntity = playerEntity;
        }
    }
}
