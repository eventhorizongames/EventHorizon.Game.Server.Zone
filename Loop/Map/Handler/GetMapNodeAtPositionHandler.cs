using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Loop.State;
using EventHorizon.Game.Server.Zone.Map;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Loop.Map.Handler
{
    public class GetMapNodeAtPositionHandler : IRequestHandler<GetMapNodeAtPositionEvent, MapNode>
    {
        readonly IServerState _serverState;
        public GetMapNodeAtPositionHandler(IServerState serverState)
        {
            _serverState = serverState;
        }
        public async Task<MapNode> Handle(GetMapNodeAtPositionEvent request, CancellationToken cancellationToken)
        {
            return (await _serverState.Map())
                .GetClosestNode(request.Position);
        }
    }
}