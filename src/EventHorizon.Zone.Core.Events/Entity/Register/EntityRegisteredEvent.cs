using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Entity.Register
{
    public struct EntityRegisteredEvent : INotification
    {
        public IObjectEntity Entity { get; set; }
    }
}