using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Math;

namespace EventHorizon.Game.Server.Zone.Entity.State
{
    public interface IEntitySearchTree
    {
        void UpdateDimensions(Vector3 dimensions);
        void Update(SearchEntity searchEntity);
        Task<IList<SearchEntity>> SearchInArea(Vector3 searchPositionCenter, float searchRadius);
        Task<IEnumerable<SearchEntity>> SearchInAreaWithTag(Vector3 searchPositionCenter, float searchRadius, IList<string> tagList);
        Task<IEnumerable<SearchEntity>> SearchInAreaWithAllTags(Vector3 searchPositionCenter, float searchRadius, IList<string> tagList);
    }
}