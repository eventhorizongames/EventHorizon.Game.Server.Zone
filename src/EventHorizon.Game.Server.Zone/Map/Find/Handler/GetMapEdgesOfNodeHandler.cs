using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Map;
using EventHorizon.Game.Server.Zone.State;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Map.Handler
{
    public class GetMapEdgesOfNodeHandler : IRequestHandler<GetMapEdgesOfNodeEvent, IList<MapEdge>>
    {
        readonly IServerState _serverState;
        public GetMapEdgesOfNodeHandler(IServerState serverState)
        {
            _serverState = serverState;
        }

        public async Task<IList<MapEdge>> Handle(GetMapEdgesOfNodeEvent request, CancellationToken cancellationToken)
        {
            return (await _serverState.Map())
                .GetEdgesOfNode(request.NodeIndex);
        }
    }
}