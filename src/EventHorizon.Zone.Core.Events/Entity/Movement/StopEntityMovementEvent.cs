namespace EventHorizon.Zone.Core.Events.Entity.Movement
{
    using MediatR;

    public struct StopEntityMovementEvent : INotification
    {
        public long EntityId { get; set; }
    }
}
