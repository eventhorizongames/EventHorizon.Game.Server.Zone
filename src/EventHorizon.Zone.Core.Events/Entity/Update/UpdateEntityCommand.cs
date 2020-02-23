using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Entity.Update
{
    public struct UpdateEntityCommand : IRequest
    {
        public EntityAction Action { get; }
        public IObjectEntity Entity { get; }

        public UpdateEntityCommand(
            EntityAction action,
            IObjectEntity entity
        )
        {
            Action = action;
            Entity = entity;
        }
    }
}