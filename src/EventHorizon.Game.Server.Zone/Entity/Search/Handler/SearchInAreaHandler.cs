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
    public class SearchInAreaHandler : IRequestHandler<SearchInAreaEvent, IEnumerable<long>>
    {
        readonly IEntitySearchTree _entitySearchTree;
        public SearchInAreaHandler(IEntitySearchTree entitySearchTree)
        {
            _entitySearchTree = entitySearchTree;
        }

        public async Task<IEnumerable<long>> Handle(SearchInAreaEvent notification, CancellationToken none)
        {
            var list = await _entitySearchTree.SearchInArea(notification.SearchPositionCenter, notification.SearchRadius) ?? new List<SearchEntity>();
            return list.Select(a => a.EntityId);
        }
    }
}