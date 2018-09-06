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
    public class FindEntitiesInAreaHandler : IRequestHandler<FindEntitiesInAreaEvent, IEnumerable<long>>
    {
        readonly IEntitySearchTree _entitySearchTree;
        public FindEntitiesInAreaHandler(IEntitySearchTree entitySearchTree)
        {
            _entitySearchTree = entitySearchTree;
        }

        public async Task<IEnumerable<long>> Handle(FindEntitiesInAreaEvent notification, CancellationToken none)
        {
            var list = await _entitySearchTree.FindEntitiesInArea(notification.SearchPositionCenter, notification.SearchRadius) ?? new List<SearchEntity>();
            return list.Select(a => a.EntityId);
        }
    }
}