using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Entity.State;
using EventHorizon.Zone.Core.Events.Entity.Action;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Zone.Core.Entity.Search
{
    public class EntityPositionChangedHandler : INotificationHandler<EntityActionEvent>
    {
        readonly EntitySearchTree _searchTree;
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
            ))
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
                    entity.Position.CurrentPosition,
                    entity.TagList
                )
            );
            _searchTree.Add(
                new SearchEntity(
                    entity.Id,
                    entity.Position.CurrentPosition,
                    entity.TagList
                )
            );
        }
    }
}