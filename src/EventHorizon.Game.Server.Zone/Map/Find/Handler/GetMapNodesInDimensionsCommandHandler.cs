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
    public class GetMapNodesInDimensionsCommandHandler
        : IRequestHandler<GetMapNodesInDimensionsCommand, IList<MapNode>>
    {
        readonly IServerState _serverState;
        public GetMapNodesInDimensionsCommandHandler(
            IServerState serverState
        )
        {
            _serverState = serverState;
        }
        public async Task<IList<MapNode>> Handle(
            GetMapNodesInDimensionsCommand request,
            CancellationToken cancellationToken
        )
        {
            return (await _serverState.Map())
                .GetClosestNodesInDimension(
                    request.Position,
                    request.Dimensions
                );
        }
    }
}