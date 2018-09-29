using EventHorizon.Game.Server.Zone.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Action
{
    public struct EntityActionEvent : INotification
    {
        public EntityAction Action { get; set; }
        public IObjectEntity Entity { get; set; }
    }
}