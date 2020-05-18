namespace EventHorizon.Zone.Core.Events.Entity.Register
{
    using MediatR;

    public struct EntityUnRegisteredEvent : INotification
    {
        public long EntityId { get; set; }

        public EntityUnRegisteredEvent(
            long entityId
        )
        {
            EntityId = entityId;
        }
    }
}