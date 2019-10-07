using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Model.Math;

namespace EventHorizon.Zone.Core.Entity.State
{
    public class InMemoryEntitySearchTree : EntitySearchTree
    {
        private static Octree<SearchEntity> _searchOctree = new Octree<SearchEntity>(
            Vector3.Zero,
            new Vector3(
                128,
                128,
                128
            ),
            0
        );
        public void Add(
            SearchEntity searchEntity
        )
        {
            _searchOctree.Add(
                searchEntity
            );
        }
        public void Remove(
            SearchEntity entity
        )
        {
            _searchOctree.Remove(
                entity
            );
        }
        public void Reset()
        {
            _searchOctree = new Octree<SearchEntity>(
                Vector3.Zero,
                new Vector3(128, 128, 128),
                0
            );
        }

        public void UpdateDimensions(
            Vector3 dimensions
        )
        {
            var newSearchOctree = new Octree<SearchEntity>(
                new Vector3(
                    -(dimensions.X / 2),
                    -(dimensions.Y / 2),
                    -(dimensions.Z / 2)
                ),
                dimensions,
                0
            );
            foreach (var node in _searchOctree.All())
            {
                newSearchOctree.Add(
                    node
                );
            }
            _searchOctree = newSearchOctree;
        }

        /// <summary>
        /// Search for a list of entities that are contained in the search area.
        /// </summary>
        /// <param name="searchPositionCenter"></param>
        /// <param name="searchRadius"></param>
        /// <returns></returns>
        public Task<IList<SearchEntity>> SearchInArea(
            Vector3 searchPositionCenter,
            float searchRadius
        )
        {
            return Task.FromResult(
                _searchOctree.FindNearbyPoints(
                    searchPositionCenter,
                    searchRadius
                )
            );
        }

        /// <summary>
        /// Search for a list of entities that are contained in the search area with any one tag from the provided list. 
        /// </summary>
        /// <param name="searchPositionCenter"></param>
        /// <param name="searchRadius"></param>
        /// <param name="tagList"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SearchEntity>> SearchInAreaWithTag(
            Vector3 searchPositionCenter,
            float searchRadius,
            IList<string> tagList
        )
        {
            if (tagList == null
                || tagList.Count == 0)
            {
                return new List<SearchEntity>();
            }
            return (await SearchInArea(
                searchPositionCenter, searchRadius
            )).Where(
                entity => entity.TagList.Any(
                    tagList.Contains
                )
            );
        }

        /// <summary>
        /// Search for a list of entities that are contained in the search area with any one tag from the provided list. 
        /// </summary>
        /// <param name="searchPositionCenter"></param>
        /// <param name="searchRadius"></param>
        /// <param name="tagList"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SearchEntity>> SearchInAreaWithAllTags(
            Vector3 searchPositionCenter,
            float searchRadius,
            IList<string> tagList
        )
        {
            if (tagList == null
                || tagList.Count == 0)
            {
                return new List<SearchEntity>();
            }
            return (await SearchInArea(
                searchPositionCenter, searchRadius
            )).Where(
                entity => tagList.All(
                    entity.TagList.Contains
                )
            );
        }

        public Task<IList<SearchEntity>> SearchInArea(
            Vector3 searchPositionCenter,
            Vector3 searchDimension
        ) => (Task.FromResult(
            _searchOctree.FindNearbyPoints(
                searchPositionCenter,
                searchDimension
            )
        ));
    }
}