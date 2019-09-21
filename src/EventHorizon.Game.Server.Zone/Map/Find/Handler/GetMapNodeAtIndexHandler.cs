using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Model.Map;
using EventHorizon.Game.Server.Zone.State;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Map.Handler
{
    public class GetMapNodeAtIndexHandler : IRequestHandler<GetMapNodeAtIndexEvent, MapNode>
    {
        readonly IServerState _serverState;
        public GetMapNodeAtIndexHandler(IServerState serverState)
        {
            _serverState = serverState;
        }
        public async Task<MapNode> Handle(GetMapNodeAtIndexEvent request, CancellationToken cancellationToken)
        {
            return (await _serverState.Map())
                .GetNode(request.NodeIndex);
        }
    }
}