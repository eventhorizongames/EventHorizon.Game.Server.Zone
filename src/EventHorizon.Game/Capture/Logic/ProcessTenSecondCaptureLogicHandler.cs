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

    public class ProcessTenSecondCaptureLogicHandler
        : IRequestHandler<ProcessTenSecondCaptureLogic>
    {
        private readonly IMediator _mediator;

        public ProcessTenSecondCaptureLogicHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(
            ProcessTenSecondCaptureLogic request,
            CancellationToken cancellationToken
        )
        {
            var playerEntity = request.PlayerEntity;

            await _mediator.Publish(
                ClientActionShowTenSecondCaptureMessageToSingleEvent.Create(
                    playerEntity.ConnectionId,
                    new ClientActionShowTenSecondCaptureMessageData()
                )
            );

            var captureState = playerEntity.GetProperty<GamePlayerCaptureState>(
                GamePlayerCaptureState.PROPERTY_NAME
            );
            captureState.ShownTenSecondMessage = true;
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
