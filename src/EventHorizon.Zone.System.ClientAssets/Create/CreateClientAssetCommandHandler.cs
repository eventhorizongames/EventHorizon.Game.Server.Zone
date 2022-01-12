namespace EventHorizon.Zone.System.ClientAssets.Create;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.ClientAssets.Api;
using EventHorizon.Zone.System.ClientAssets.Events.Create;
using EventHorizon.Zone.System.ClientAssets.Save;

using global::System;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class CreateClientAssetCommandHandler
    : IRequestHandler<
          CreateClientAssetCommand,
          StandardCommandResult
      >
{
    private readonly IMediator _mediator;
    private readonly ServerInfo _serverInfo;
    private readonly ClientAssetRepository _repository;

    public CreateClientAssetCommandHandler(
        IMediator mediator,
        ServerInfo serverInfo,
        ClientAssetRepository repository
    )
    {
        _mediator = mediator;
        _serverInfo = serverInfo;
        _repository = repository;
    }

    public async Task<StandardCommandResult> Handle(
        CreateClientAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        var clientAsset = request.ClientAsset;
        clientAsset.Id = Guid.NewGuid().ToString();

        if (!clientAsset.TryGetFileFullName(out _))
        {
            clientAsset.SetFileFullName(
                clientAsset.GetFileFullName(
                    _serverInfo.ClientPath
                )
            );
        }

        _repository.Set(clientAsset);

        await _mediator.Publish(
            new RunSaveClientAssetsEvent(),
            cancellationToken
        );

        return new();
    }
}
