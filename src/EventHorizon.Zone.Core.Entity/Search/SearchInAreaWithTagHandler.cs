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

    public class SearchInAreaWithTagHandler
        : IRequestHandler<SearchInAreaWithTagEvent, IEnumerable<long>>
    {
        private readonly EntitySearchTree _entitySearchTree;

        public SearchInAreaWithTagHandler(
            EntitySearchTree entitySearchTree
        )
        {
            _entitySearchTree = entitySearchTree;
        }

        public async Task<IEnumerable<long>> Handle(
            SearchInAreaWithTagEvent notification,
            CancellationToken none
        )
        {
            return (
                await _entitySearchTree.SearchInAreaWithTag(
                    notification.SearchPositionCenter,
                    notification.SearchRadius,
                    notification.TagList
                ) ?? new List<SearchEntity>()
            ).Select(
                mock => mock.EntityId
            );
        }
    }
}
