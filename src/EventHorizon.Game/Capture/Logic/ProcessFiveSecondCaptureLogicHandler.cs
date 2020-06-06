namespace EventHorizon.Game.Capture.Logic
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Client;
    using EventHorizon.Game.Model;
    using EventHorizon.Game.Model.Client;
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using EventHorizon.Zone.Core.Model.Entity;
    using MediatR;

    public class ProcessFiveSecondCaptureLogicHandler 
        : IRequestHandler<ProcessFiveSecondCaptureLogic>
    {
        private readonly IMediator _mediator;

        public ProcessFiveSecondCaptureLogicHandler(
            IMediator mediator
        )
        {
            this._mediator = mediator;
        }

        public async Task<Unit> Handle(
            ProcessFiveSecondCaptureLogic request, 
            CancellationToken cancellationToken
        )
        {
            var playerEntity = request.PlayerEntity;

            await _mediator.Publish(
                ClientActionShowFiveSecondCaptureMessageToSingleEvent.Create(
                    playerEntity.ConnectionId,
                    new ClientActionShowFiveSecondCaptureMessageData()
                ),
                cancellationToken
            );

            var captureState = playerEntity.GetProperty<GamePlayerCaptureState>(
                GamePlayerCaptureState.PROPERTY_NAME
            );
            captureState.ShownFiveSecondMessage = true;
            playerEntity = playerEntity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                captureState
            );

            await _mediator.Send(
                new UpdateEntityCommand(
                    EntityAction.PROPERTY_CHANGED,
                    playerEntity
                ),
                cancellationToken
            );

            return Unit.Value;
        }
    }
}
