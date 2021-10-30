namespace EventHorizon.Zone.Core.Events.Entity.Data
{
    using EventHorizon.Zone.Core.Model.Entity;

    using MediatR;

    public struct PrePopulateEntityDataEvent
        : INotification
    {
        public IObjectEntity Entity { get; }

        public PrePopulateEntityDataEvent(
            IObjectEntity entity
        )
        {
            Entity = entity;
        }
    }
}
