namespace EventHorizon.Zone.Core.Events.Entity.Search;

using System.Collections.Generic;
using System.Numerics;

using MediatR;

public class SearchInBoundingAreaCommand : IRequest<IEnumerable<long>>
{
    public Vector3 SearchPositionCenter { get; set; }
    public Vector3 SearchDimension { get; set; }
}
