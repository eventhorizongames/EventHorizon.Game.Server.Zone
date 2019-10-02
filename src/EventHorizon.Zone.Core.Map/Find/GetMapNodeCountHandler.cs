using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Map;
using MediatR;
using EventHorizon.Zone.Core.Model.Map;

namespace EventHorizon.Zone.Core.Map.Find
{
    public class GetMapNodeCountHandler : IRequestHandler<GetMapNodeCountEvent, int>
    {
        readonly IMapGraph _map;
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
            return Task.FromResult(
                _map.NumberOfNodes
            );
        }
    }
}