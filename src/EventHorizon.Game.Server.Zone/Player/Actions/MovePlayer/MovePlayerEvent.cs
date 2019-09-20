using System.Numerics;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Game.Server.Zone.Player.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer
{
    public class MovePlayerEvent : IRequest<Vector3>
    {
        public PlayerEntity Player { get; set; }
        public long MoveDirection { get; set; }
    }
}