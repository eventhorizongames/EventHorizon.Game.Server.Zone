namespace EventHorizon.Zone.Core.Map.Find;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Model.Map;

using MediatR;

public class GetMapNodeCountHandler
    : IRequestHandler<GetMapNodeCountEvent, int>
{
    private readonly IMapGraph _map;

    public GetMapNodeCountHandler(
        IMapGraph map
    )
    {
        _map = map;
    }

    public Task<int> Handle(
        GetMapNodeCountEvent request,
        CancellationToken cancellationToken
    )
    {
        return _map.NumberOfNodes.FromResult();
    }
}
