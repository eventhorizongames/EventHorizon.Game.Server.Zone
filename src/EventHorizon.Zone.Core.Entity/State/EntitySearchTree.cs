namespace EventHorizon.Zone.Core.Entity.State;

using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

using EventHorizon.Game.Server.Zone.Entity.Model;

public interface EntitySearchTree
{
    void UpdateDimensions(
        Vector3 dimensions
    );
    void Add(
        SearchEntity searchEntity
    );
    void Remove(
        SearchEntity searchEntity
    );
    Task<IList<SearchEntity>> SearchInArea(
        Vector3 searchPositionCenter,
        float searchRadius
    );
    Task<IList<SearchEntity>> SearchInArea(
        Vector3 searchPositionCenter,
        Vector3 searchDimension
    );
    Task<IEnumerable<SearchEntity>> SearchInAreaWithTag(
        Vector3 searchPositionCenter,
        float searchRadius,
        IList<string> tagList
    );
    Task<IEnumerable<SearchEntity>> SearchInAreaWithAllTags(
        Vector3 searchPositionCenter,
        float searchRadius,
        IList<string> tagList
    );
}
