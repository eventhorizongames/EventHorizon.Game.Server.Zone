namespace EventHorizon.Zone.System.ClientAssets.Query;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.ClientAssets.Api;
using EventHorizon.Zone.System.ClientAssets.Events.Query;
using EventHorizon.Zone.System.ClientAssets.Model;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class QueryForClientAssetByIdHandler
    : IRequestHandler<
          QueryForClientAssetById,
          CommandResult<ClientAsset>
      >
{
    private readonly ClientAssetRepository _repository;

    public QueryForClientAssetByIdHandler(
        ClientAssetRepository repository
    )
    {
        _repository = repository;
    }

    public Task<CommandResult<ClientAsset>> Handle(
        QueryForClientAssetById request,
        CancellationToken cancellationToken
    )
    {
        var clientAssetOption = _repository.Get(request.Id);

        if (clientAssetOption)
        {
            return new CommandResult<ClientAsset>(
                clientAssetOption.Value
            ).FromResult();
        }

        return new CommandResult<ClientAsset>(
            "CLIENT_ASSET_NOT_FOUND"
        ).FromResult();
    }
}
