namespace EventHorizon.Zone.Core.Entity.Search;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Entity.State;
using EventHorizon.Zone.Core.Events.Entity.Action;
using EventHorizon.Zone.Core.Model.Entity;

using MediatR;

public class EntityPositionChangedHandler
    : INotificationHandler<EntityActionEvent>
{
    private readonly EntitySearchTree _searchTree;

    public EntityPositionChangedHandler(
        EntitySearchTree searchTree
    )
    {
        _searchTree = searchTree;
    }

    public Task Handle(
        EntityActionEvent notification,
        CancellationToken cancellationToken
    )
    {
        if (notification.Action.Equals(
            EntityAction.POSITION
        ) && notification.Entity is not null)
        {
            // Update the Entity Search Tree
            SendSearchEntityUpdate(
                notification.Entity
            );
        }
        return Task.CompletedTask;
    }

    private void SendSearchEntityUpdate(
        IObjectEntity entity
    )
    {
        _searchTree.Remove(
            new SearchEntity(
                entity.Id,
                entity.Transform.Position,
                entity.TagList
            )
        );
        _searchTree.Add(
            new SearchEntity(
                entity.Id,
                entity.Transform.Position,
                entity.TagList
            )
        );
    }
}
