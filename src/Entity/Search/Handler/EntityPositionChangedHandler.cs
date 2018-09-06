using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Action;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.State;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Search.Handler
{
    public class EntityPositionChangedHandler : INotificationHandler<EntityActionEvent>
    {
        readonly IEntitySearchTree _searchTree;
        public EntityPositionChangedHandler(IEntitySearchTree searchTree)
        {
            _searchTree = searchTree;
        }
        public Task Handle(EntityActionEvent notification, CancellationToken cancellationToken)
        {
            if (notification.Action.Equals(EntityAction.POSITION))
            {
                // Update the Entity Search Tree
                SendSearchEntityUpdate(notification.Entity);
            }
            return Task.CompletedTask;
        }

        private void SendSearchEntityUpdate(IObjectEntity entity)
        {
            _searchTree.Update(new SearchEntity(entity.Id, entity.Position.CurrentPosition, entity.TagList));
        }
    }
}