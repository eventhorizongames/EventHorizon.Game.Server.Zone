namespace EventHorizon.Zone.System.DataStorage.Create
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.DataStorage.Api;
    using EventHorizon.Zone.System.DataStorage.Events.Create;
    using EventHorizon.Zone.System.DataStorage.Save;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class CreateDataStoreValueCommandHandler
        : IRequestHandler<CreateDataStoreValueCommand, StandardCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly DataStoreManagement _dataStore;

        public CreateDataStoreValueCommandHandler(
            IMediator mediator,
            DataStoreManagement dataStore
        )
        {
            _mediator = mediator;
            _dataStore = dataStore;
        }

        public async Task<StandardCommandResult> Handle(
            CreateDataStoreValueCommand request,
            CancellationToken cancellationToken
        )
        {
            _dataStore.Set(
                request.Key,
                request.Value
            );

            _dataStore.UpdateSchema(
                request.Key,
                request.Type
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
