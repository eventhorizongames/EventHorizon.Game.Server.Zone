namespace EventHorizon.Zone.Core.Entity.Search;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Entity.State;
using EventHorizon.Zone.Core.Events.Entity.Search;

using MediatR;

public class SearchInAreaWithAllTagsHandler
    : IRequestHandler<SearchInAreaWithAllTagsEvent, IEnumerable<long>>
{
    private readonly EntitySearchTree _entitySearchTree;

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
