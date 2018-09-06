using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.State;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Find.Handler
{
    public class FindAnyEntitiesWithATagFromListHandler : IRequestHandler<FindAnyEntitiesWithATagFromListEvent, IEnumerable<long>>
    {
        readonly IEntitySearchTree _entitySearchTree;
        public FindAnyEntitiesWithATagFromListHandler(IEntitySearchTree entitySearchTree)
        {
            _entitySearchTree = entitySearchTree;
        }

        public async Task<IEnumerable<long>> Handle(FindAnyEntitiesWithATagFromListEvent notification, CancellationToken none)
        {
            var list = await _entitySearchTree.FindAnyEntitiesWithATagFromList(notification.SearchPositionCenter, notification.SearchRadius, notification.TagList) ?? new List<SearchEntity>();
            return list.Select(a => a.EntityId);
        }
    }
}