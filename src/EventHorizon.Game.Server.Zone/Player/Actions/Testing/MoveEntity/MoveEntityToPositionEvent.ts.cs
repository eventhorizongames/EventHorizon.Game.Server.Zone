using System.Numerics;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Actions.Testing.MoveEntity
{
    public class MoveEntityToPositionEvent : INotification
    {
        public long EntityId { get; set; }
        public Vector3 Position { get; set; }
    }
}