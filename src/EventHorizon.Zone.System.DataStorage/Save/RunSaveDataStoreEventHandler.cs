namespace EventHorizon.Zone.System.DataStorage.Save
{
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class RunSaveDataStoreEventHandler
        : INotificationHandler<RunSaveDataStoreEvent>
    {
        private readonly IMediator _mediator;

        public RunSaveDataStoreEventHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            RunSaveDataStoreEvent request,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new SaveDataStoreCommand(),
                cancellationToken
            );
        }
    }
}
