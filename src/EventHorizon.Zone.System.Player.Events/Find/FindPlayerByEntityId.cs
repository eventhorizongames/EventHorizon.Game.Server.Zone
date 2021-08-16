namespace EventHorizon.Zone.System.Player.Events.Find
{
    using EventHorizon.Zone.Core.Model.Player;

    using MediatR;

    public struct FindPlayerByEntityId : IRequest<PlayerEntity>
    {
        public long EntityId { get; }

        public FindPlayerByEntityId(
            long entityId
        )
        {
            EntityId = entityId;
        }
    }
}
