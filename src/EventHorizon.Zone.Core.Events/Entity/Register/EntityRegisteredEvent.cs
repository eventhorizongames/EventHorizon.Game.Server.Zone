namespace EventHorizon.Zone.Core.Events.Entity.Register
{
    using EventHorizon.Zone.Core.Model.Entity;

    using MediatR;

    public struct EntityRegisteredEvent : INotification
    {
        public IObjectEntity Entity { get; set; }
    }
}
