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
    public class SearchInAreaWithAllTagsHandler : IRequestHandler<SearchInAreaWithAllTagsEvent, IEnumerable<long>>
    {
        readonly IEntitySearchTree _entitySearchTree;
        public SearchInAreaWithAllTagsHandler(IEntitySearchTree entitySearchTree)
        {
            _entitySearchTree = entitySearchTree;
        }

        public async Task<IEnumerable<long>> Handle(SearchInAreaWithAllTagsEvent notification, CancellationToken none)
        {
            var list = await _entitySearchTree.SearchInAreaWithAllTags(notification.SearchPositionCenter, notification.SearchRadius, notification.TagList) ?? new List<SearchEntity>();
            return list.Select(a => a.EntityId);
        }
    }
}