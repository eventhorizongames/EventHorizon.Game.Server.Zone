using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.State;
using MediatR;
using EventHorizon.Game.Server.Zone.Events.Entity.Search;

namespace EventHorizon.Game.Server.Zone.Entity.Search.Handler
{
    public class SearchInBoundingAreaHandler : IRequestHandler<SearchInBoundingAreaCommand, IEnumerable<long>>
    {
        readonly IEntitySearchTree _entitySearchTree;
        public SearchInBoundingAreaHandler(
            IEntitySearchTree entitySearchTree
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
                a => a.EntityId
            );
        }
    }
}