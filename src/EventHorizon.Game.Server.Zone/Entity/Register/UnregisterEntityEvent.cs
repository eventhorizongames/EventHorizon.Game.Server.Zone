using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Register
{
    public struct UnregisterEntityEvent : INotification
    {
        public IObjectEntity Entity { get; set; }
    }
}