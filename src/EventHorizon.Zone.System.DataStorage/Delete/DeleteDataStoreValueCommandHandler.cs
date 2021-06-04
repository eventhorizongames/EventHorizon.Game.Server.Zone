namespace EventHorizon.Zone.System.DataStorage.Delete
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.DataStorage.Api;
    using EventHorizon.Zone.System.DataStorage.Events.Delete;
    using EventHorizon.Zone.System.DataStorage.Save;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class DeleteDataStoreValueCommandHandler
        : IRequestHandler<DeleteDataStoreValueCommand, StandardCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly DataStoreManagement _dataStore;

        public DeleteDataStoreValueCommandHandler(
            IMediator mediator,
            DataStoreManagement dataStore
        )
        {
            _mediator = mediator;
            _dataStore = dataStore;
        }

        public async Task<StandardCommandResult> Handle(
            DeleteDataStoreValueCommand request,
            CancellationToken cancellationToken
        )
        {
            _dataStore.Delete(
                request.Key
            );

            _dataStore.DeleteFromSchema(
                request.Key
            );

            await _mediator.Send(
                new SaveDataStoreCommand(),
                cancellationToken
            );

            // TODO: Create Client Admin Action (ClientAdminActionDataStoreValueChanged)

            return new();
        }
    }
}
