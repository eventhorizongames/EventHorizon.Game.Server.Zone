using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Entity.State;
using MediatR;
using EventHorizon.Zone.Core.Events.Entity.Search;

namespace EventHorizon.Zone.Core.Entity.Search
{
    public struct SearchInAreaWithAllTagsHandler : IRequestHandler<SearchInAreaWithAllTagsEvent, IEnumerable<long>>
    {
        readonly EntitySearchTree _entitySearchTree;
        public SearchInAreaWithAllTagsHandler(
            EntitySearchTree entitySearchTree
        )
        {
            _entitySearchTree = entitySearchTree;
        }

        public async Task<IEnumerable<long>> Handle(
            SearchInAreaWithAllTagsEvent notification,
            CancellationToken none
        )
        {
            return (
                await _entitySearchTree.SearchInAreaWithAllTags(
                    notification.SearchPositionCenter,
                    notification.SearchRadius,
                    notification.TagList
                ) ?? new List<SearchEntity>()
            ).Select(
                entity => entity.EntityId
            );
        }
    }
}