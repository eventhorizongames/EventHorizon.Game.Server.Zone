using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Load.Map;
using EventHorizon.Game.Server.Zone.Map;
using EventHorizon.Game.Server.Zone.Map.Model;
using EventHorizon.Game.Server.Zone.State;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Map.Handler
{
    public class GetMapNodesAroundPositionHandler : IRequestHandler<GetMapNodesAroundPositionEvent, IList<MapNode>>
    {
        readonly IServerState _serverState;
        readonly IZoneMapFactory _zoneMapFactory;
        public GetMapNodesAroundPositionHandler(IServerState serverState, IZoneMapFactory zoneMapFactory)
        {
            _serverState = serverState;
            _zoneMapFactory = zoneMapFactory;
        }
        public async Task<IList<MapNode>> Handle(GetMapNodesAroundPositionEvent request, CancellationToken cancellationToken)
        {
            return (await _serverState.Map())
                .GetClosestNodes(request.Position, request.Distance * _zoneMapFactory.Map.TileDimensions);
        }
    }
}