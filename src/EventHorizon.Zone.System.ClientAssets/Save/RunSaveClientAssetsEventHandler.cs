namespace EventHorizon.Zone.System.ClientAssets.Save
{
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class RunSaveClientAssetsEventHandler
        : INotificationHandler<RunSaveClientAssetsEvent>
    {
        private readonly IMediator _mediator;

        public RunSaveClientAssetsEventHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            RunSaveClientAssetsEvent notification,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new SaveClientAssetsCommand(),
                cancellationToken
            );
        }
    }
}
