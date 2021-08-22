namespace EventHorizon.Zone.Core.Events.Entity.Data
{
    using EventHorizon.Zone.Core.Model.Entity;

    using MediatR;

    public struct PopulateEntityDataEvent : INotification
    {
        public IObjectEntity Entity { get; set; }
    }
}
