namespace EventHorizon.Zone.System.ClientAssets.Update
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.ClientAssets.Events.Update;
    using EventHorizon.Zone.System.ClientAssets.Save;
    using EventHorizon.Zone.System.ClientAssets.State.Api;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class UpdateClientAssetCommandHandler
        : IRequestHandler<UpdateClientAssetCommand, StandardCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly ClientAssetRepository _repository;

        public UpdateClientAssetCommandHandler(
            IMediator mediator,
            ClientAssetRepository repository
        )
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<StandardCommandResult> Handle(
            UpdateClientAssetCommand request,
            CancellationToken cancellationToken
        )
        {
            _repository.Set(
                request.ClientAsset
            );

            await _mediator.Publish(
                new RunSaveClientAssetsEvent(),
                cancellationToken
            );

            return new();
        }
    }
}
