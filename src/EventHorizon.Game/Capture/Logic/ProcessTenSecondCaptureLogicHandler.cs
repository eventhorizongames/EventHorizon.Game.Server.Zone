namespace EventHorizon.Game.Capture.Logic
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Client;
    using EventHorizon.Game.Model.Client;
    using MediatR;

    public class ProcessTenSecondCaptureLogicHandler : IRequestHandler<ProcessTenSecondCaptureLogic>
    {
        private readonly IMediator _mediator;

        public ProcessTenSecondCaptureLogicHandler(
            IMediator mediator
        )
        {
            _mediator= mediator;
        }

        public async Task<Unit> Handle(
            ProcessTenSecondCaptureLogic request,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Publish(
                ClientActionShowTenSecondCaptureMessageToSingleEvent.Create(
                    request.PlayerEntity.ConnectionId,
                    new ClientActionShowTenSecondCaptureMessageData()
                )
            );

            return Unit.Value;
        }
    }
}
