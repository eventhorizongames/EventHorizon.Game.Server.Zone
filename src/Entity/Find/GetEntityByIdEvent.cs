using EventHorizon.Game.Server.Zone.Entity.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Find
{
    public struct GetEntityByIdEvent : IRequest<IObjectEntity>
    {
        public long EntityId { get; set; }
    }
}