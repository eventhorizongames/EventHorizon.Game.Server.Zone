using EventHorizon.Zone.Core.Model.Entity;

using MediatR;

namespace EventHorizon.Zone.Core.Events.Entity.Data
{
    public struct PopulateEntityDataEvent : INotification
    {
        public IObjectEntity Entity { get; set; }
    }
}
