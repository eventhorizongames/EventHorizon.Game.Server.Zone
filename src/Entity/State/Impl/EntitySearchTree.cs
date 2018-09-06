using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Math;

namespace EventHorizon.Game.Server.Zone.Entity.State.Impl
{
    public class EntitySearchTree : IEntitySearchTree
    {
        public static Octree<SearchEntity> SEARCH_OCTREE = new Octree<SearchEntity>(Vector3.Zero, new Vector3(128, 128, 128), 0);
        public void Update(SearchEntity searchEntity)
        {
            SEARCH_OCTREE.Remove(searchEntity);
            SEARCH_OCTREE.Add(searchEntity);
        }

        public void UpdateDimensions(Vector3 dimensions)
        {
            var newSearchOctree = new Octree<SearchEntity>(Vector3.Zero, dimensions, 0);
            foreach (var node in SEARCH_OCTREE.All())
            {
                newSearchOctree.Add(node);
            }
            SEARCH_OCTREE = newSearchOctree;
        }

        /// <summary>
        /// Search for a list of entities that are contained in the search area.
        /// </summary>
        /// <param name="searchPositionCenter"></param>
        /// <param name="searchRadius"></param>
        /// <returns></returns>
        public Task<IList<SearchEntity>> SearchInArea(Vector3 searchPositionCenter, float searchRadius)
        {
            return Task.FromResult(SEARCH_OCTREE.FindNearbyPoints(searchPositionCenter, searchRadius));
        }

        /// <summary>
        /// Search for a list of entities that are contained in the search area with any one tag from the provided list. 
        /// </summary>
        /// <param name="searchPositionCenter"></param>
        /// <param name="searchRadius"></param>
        /// <param name="tagList"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SearchEntity>> SearchInAreaWithTag(Vector3 searchPositionCenter, float searchRadius, IList<string> tagList)
        {
            tagList = tagList ?? new List<string>();
            return (await SearchInArea(searchPositionCenter, searchRadius))
                .Where(entity => entity.TagList?.Any(tagList.Contains) ?? false);
        }
    }
}