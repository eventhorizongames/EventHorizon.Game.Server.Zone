namespace EventHorizon.Zone.System.ClientAssets.Reload;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.ClientAssets.Api;
using EventHorizon.Zone.System.ClientAssets.ClientActions;
using EventHorizon.Zone.System.ClientAssets.Load;
using EventHorizon.Zone.System.ClientAssets.Query;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class ReloadClientAssetsSystemCommandHandler
    : IRequestHandler<
          ReloadClientAssetsSystemCommand,
          StandardCommandResult
      >
{
    private readonly ISender _sender;
    private readonly IPublisher _publisher;
    private readonly ClientAssetRepository _clientAssetRepository;

    public ReloadClientAssetsSystemCommandHandler(
        ISender sender,
        IPublisher publisher,
        ClientAssetRepository clientAssetRepository
    )
    {
        _sender = sender;
        _publisher = publisher;
        _clientAssetRepository = clientAssetRepository;
    }

    public async Task<StandardCommandResult> Handle(
        ReloadClientAssetsSystemCommand request,
        CancellationToken cancellationToken
    )
    {
        _clientAssetRepository.Clear();

        await _sender.Send(
            new LoadSystemClientAssetsCommand(),
            cancellationToken
        );

        await _publisher.Publish(
            ClientAssetsSystemReloadClientActionToAllEvent.Create(
                await _sender.Send(
                    new QueryForClientAssetList(),
                    cancellationToken
                )
            ),
            cancellationToken
        );

        return new();
    }
}
