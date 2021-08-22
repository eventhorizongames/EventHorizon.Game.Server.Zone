namespace EventHorizon.Zone.Core.Events.Entity.Register
{
    using EventHorizon.Zone.Core.Model.Entity;

    using MediatR;

    public struct UnRegisterEntityEvent : INotification
    {
        public IObjectEntity Entity { get; }

        public UnRegisterEntityEvent(
            IObjectEntity entity
        )
        {
            Entity = entity;
        }
    }
}
