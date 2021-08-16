namespace EventHorizon.Zone.Core.Entity.Search
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Game.Server.Zone.Entity.Model;
    using EventHorizon.Zone.Core.Entity.State;
    using EventHorizon.Zone.Core.Events.Entity.Search;

    using MediatR;

    public class SearchInAreaHandler
        : IRequestHandler<SearchInAreaEvent, IEnumerable<long>>
    {
        private readonly EntitySearchTree _entitySearchTree;

        public SearchInAreaHandler(
            EntitySearchTree entitySearchTree
        )
        {
            _entitySearchTree = entitySearchTree;
        }

        public async Task<IEnumerable<long>> Handle(
            SearchInAreaEvent notification,
            CancellationToken none
        )
        {
            return (
                await _entitySearchTree.SearchInArea(
                    notification.SearchPositionCenter,
                    notification.SearchRadius
                ) ?? new List<SearchEntity>()
            ).Select(
                entity => entity.EntityId
            );
        }
    }
}
