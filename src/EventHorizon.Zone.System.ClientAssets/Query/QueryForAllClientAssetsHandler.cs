namespace EventHorizon.Zone.System.ClientAssets.Query;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.ClientAssets.Api;
using EventHorizon.Zone.System.ClientAssets.Events.Query;
using EventHorizon.Zone.System.ClientAssets.Model;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class QueryForAllClientAssetsHandler
    : IRequestHandler<
          QueryForAllClientAssets,
          CommandResult<IEnumerable<ClientAsset>>
      >
{
    private readonly ClientAssetRepository _repository;

    public QueryForAllClientAssetsHandler(
        ClientAssetRepository repository
    )
    {
        _repository = repository;
    }

    public Task<
        CommandResult<IEnumerable<ClientAsset>>
    > Handle(
        QueryForAllClientAssets request,
        CancellationToken cancellationToken
    ) =>
        new CommandResult<IEnumerable<ClientAsset>>(
            _repository.All()
        ).FromResult();
}
