using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Entity.Find
{
    public struct GetEntityByIdEvent : IRequest<IObjectEntity>
    {
        public long EntityId { get; set; }
    }
}