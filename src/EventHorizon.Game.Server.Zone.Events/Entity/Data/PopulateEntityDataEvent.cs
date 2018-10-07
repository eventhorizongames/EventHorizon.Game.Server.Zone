
using EventHorizon.Game.Server.Zone.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Entity.Data
{
    public struct PopulateEntityDataEvent : INotification
    {
        public IObjectEntity Entity { get; set; }
    }
}