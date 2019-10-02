using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;

namespace EventHorizon.Game.Server.Zone.Entity.State
{
    public interface IEntitySearchTree
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
}