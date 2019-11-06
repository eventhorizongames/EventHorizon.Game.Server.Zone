using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using MediatR;
using EventHorizon.Zone.Core.Events.Entity.Search;
using EventHorizon.Zone.Core.Entity.State;

namespace EventHorizon.Zone.Core.Entity.Search
{
    public class SearchInBoundingAreaHandler : IRequestHandler<SearchInBoundingAreaCommand, IEnumerable<long>>
    {
        readonly EntitySearchTree _entitySearchTree;
        public SearchInBoundingAreaHandler(
            EntitySearchTree entitySearchTree
        )
        {
            _entitySearchTree = entitySearchTree;
        }

        public async Task<IEnumerable<long>> Handle(
            SearchInBoundingAreaCommand request,
            CancellationToken none
        )
        {
            return (
                await _entitySearchTree.SearchInArea(
                        request.SearchPositionCenter,
                        request.SearchDimension
                ) ?? new List<SearchEntity>()
            ).Select(
                entity => entity.EntityId
            );
        }
    }
}