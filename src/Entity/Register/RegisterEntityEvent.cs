using EventHorizon.Game.Server.Zone.Entity.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Register
{
    public struct RegisterEntityEvent : IRequest<IObjectEntity>
    {
        public IObjectEntity Entity { get; set; }
    }
}