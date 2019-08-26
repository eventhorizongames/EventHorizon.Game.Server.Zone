using System.Collections.Generic;
using System.Numerics;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Entity.Search
{
    public class SearchInBoundingAreaCommand : IRequest<IEnumerable<long>>
    {
        public Vector3 SearchPositionCenter { get; set; }
        public Vector3 SearchDimension { get; set; }
    }
}