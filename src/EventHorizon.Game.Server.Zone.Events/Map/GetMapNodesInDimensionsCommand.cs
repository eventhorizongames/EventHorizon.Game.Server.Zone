using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Map
{
    public struct GetMapNodesInDimensionsCommand : IRequest<IList<MapNode>>
    {
        public Vector3 Position { get; }
        public Vector3 Dimensions { get; }

        public GetMapNodesInDimensionsCommand(
            Vector3 position,
            Vector3 dimensions
        )
        {
            Position = position;
            Dimensions = dimensions;
        }
    }
}