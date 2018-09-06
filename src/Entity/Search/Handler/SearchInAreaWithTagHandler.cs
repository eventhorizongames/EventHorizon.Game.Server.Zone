using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.State;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Search.Handler
{
    public class SearchInAreaWithTagHandler : IRequestHandler<SearchInAreaWithTagEvent, IEnumerable<long>>
    {
        readonly IEntitySearchTree _entitySearchTree;
        public SearchInAreaWithTagHandler(IEntitySearchTree entitySearchTree)
        {
            _entitySearchTree = entitySearchTree;
        }

        public async Task<IEnumerable<long>> Handle(SearchInAreaWithTagEvent notification, CancellationToken none)
        {
            var list = await _entitySearchTree.SearchInAreaWithTag(notification.SearchPositionCenter, notification.SearchRadius, notification.TagList) ?? new List<SearchEntity>();
            return list.Select(a => a.EntityId);
        }
    }
}