namespace EventHorizon.Game.Capture.Logic
{
    using EventHorizon.Zone.Core.Model.Player;
    using MediatR;

    public struct ProcessTenSecondCaptureLogic : IRequest
    {
        public PlayerEntity PlayerEntity { get; }

        public ProcessTenSecondCaptureLogic(
            PlayerEntity playerEntity
        )
        {
            PlayerEntity = playerEntity;
        }
    }
}
