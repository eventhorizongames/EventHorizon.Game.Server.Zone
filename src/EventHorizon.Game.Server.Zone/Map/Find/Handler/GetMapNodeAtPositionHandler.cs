using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Map;
using EventHorizon.Game.Server.Zone.State;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Map.Handler
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