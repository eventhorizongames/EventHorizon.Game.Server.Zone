namespace EventHorizon.Zone.Core.Events.Entity.Find
{
    using EventHorizon.Zone.Core.Model.Entity;

    using MediatR;

    public struct GetEntityByIdEvent : IRequest<IObjectEntity>
    {
        public long EntityId { get; set; }

        public GetEntityByIdEvent(
            long entityId
        )
        {
            EntityId = entityId;
        }
    }
}
