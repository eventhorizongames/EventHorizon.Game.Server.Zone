namespace EventHorizon.Game.Capture.Logic
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Client;
    using EventHorizon.Game.Model.Client;
    using MediatR;

    public class ProcessFiveSecondCaptureLogicHandler : IRequestHandler<ProcessFiveSecondCaptureLogic>
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
            await _mediator.Publish(
                ClientActionShowFiveSecondCaptureMessageToSingleEvent.Create(
                    request.PlayerEntity.ConnectionId,
                    new ClientActionShowFiveSecondCaptureMessageData()
                ),
                cancellationToken
            );

            return Unit.Value;
        }
    }
}
