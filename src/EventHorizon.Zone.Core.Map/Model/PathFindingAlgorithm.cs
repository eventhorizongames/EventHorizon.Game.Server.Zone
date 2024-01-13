namespace EventHorizon.Zone.Core.Map.Model;

using System.Collections.Generic;
using System.Numerics;

using EventHorizon.Zone.Core.Model.Map;

public interface PathFindingAlgorithm
{
    Queue<Vector3> Search(
        IMapGraph mapGraph,
        MapNode fromMapNode,
        MapNode toMapNode
    );
}
