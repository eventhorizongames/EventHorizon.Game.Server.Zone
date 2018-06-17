using EventHorizon.Game.Server.Zone.Entity.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Registered
{
    public struct EntityRegisteredEvent : INotification
    {
        public IObjectEntity Entity { get; set; }
    }
}