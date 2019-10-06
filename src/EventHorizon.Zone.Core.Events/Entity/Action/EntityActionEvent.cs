using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Entity.Action
{
    public struct EntityActionEvent : INotification
    {
        public EntityAction Action { get; set; }
        public IObjectEntity Entity { get; set; }
    }
}