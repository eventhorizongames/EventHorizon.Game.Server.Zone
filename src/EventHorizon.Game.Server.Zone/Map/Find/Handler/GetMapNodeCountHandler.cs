using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Game.Server.Zone.State;
using EventHorizon.Game.Server.Zone.Map;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Map.Handler
{
    public class GetMapNodeCountHandler : IRequestHandler<GetMapNodeCountEvent, int>
    {
        readonly IServerState _serverState;
        public GetMapNodeCountHandler(IServerState serverState)
        {
            _serverState = serverState;
        }

        public async Task<int> Handle(GetMapNodeCountEvent request, CancellationToken cancellationToken)
        {
            return (await _serverState.Map()).NumberOfNodes;
        }
    }
}