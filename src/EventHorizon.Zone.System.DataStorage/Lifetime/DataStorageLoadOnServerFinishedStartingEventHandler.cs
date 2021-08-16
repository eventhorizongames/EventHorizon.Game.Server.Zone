namespace EventHorizon.Zone.System.DataStorage.Lifetime
{
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.System.DataStorage.Load;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class DataStorageLoadOnServerFinishedStartingEventHandler
        : INotificationHandler<ServerFinishedStartingEvent>
    {
        private readonly IMediator _mediator;

        public DataStorageLoadOnServerFinishedStartingEventHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            ServerFinishedStartingEvent notification,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new LoadDataStoreCommand(),
                cancellationToken
            );
        }
    }
}
