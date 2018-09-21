using EventHorizon.Game.Server.Zone.Entity.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Registered
{
    public struct EntityUnregisteredEvent : INotification
    {
        public long EntityId { get; set; }
    }
}