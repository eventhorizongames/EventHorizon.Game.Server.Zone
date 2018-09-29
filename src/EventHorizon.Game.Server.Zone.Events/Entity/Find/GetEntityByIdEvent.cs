using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Entity.Find
{
    public struct GetEntityByIdEvent : IRequest<IObjectEntity>
    {
        public long EntityId { get; set; }
    }
}