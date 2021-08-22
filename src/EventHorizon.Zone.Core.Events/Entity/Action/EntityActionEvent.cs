namespace EventHorizon.Zone.Core.Events.Entity.Action
{
    using EventHorizon.Zone.Core.Model.Entity;

    using MediatR;

    public struct EntityActionEvent : INotification
    {
        public EntityAction Action { get; set; }
        public IObjectEntity Entity { get; set; }

        public EntityActionEvent(
            EntityAction action,
            IObjectEntity entity
        )
        {
            Action = action;
            Entity = entity;
        }
    }
}
