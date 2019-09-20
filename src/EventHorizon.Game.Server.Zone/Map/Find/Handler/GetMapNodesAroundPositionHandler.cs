using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Map;
using EventHorizon.Game.Server.Zone.Load.Map;
using EventHorizon.Game.Server.Zone.Map;
using EventHorizon.Zone.Core.Model.Map;
using EventHorizon.Game.Server.Zone.State;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Map.Handler
{
    public class GetMapNodesAroundPositionHandler
        : IRequestHandler<GetMapNodesAroundPositionEvent, IList<MapNode>>
    {
        readonly IServerState _serverState;
        public GetMapNodesAroundPositionHandler(
            IServerState serverState
        )
        {
            _serverState = serverState;
        }
        public async Task<IList<MapNode>> Handle(
            GetMapNodesAroundPositionEvent request,
            CancellationToken cancellationToken
        )
        {
            return (await _serverState.Map())
                .GetClosestNodes(
                    request.Position,
                    request.Distance
                );
        }
    }
}