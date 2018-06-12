using System;
using System.Numerics;

namespace EventHorizon.Game.Server.Zone.Player.Model
{
    public class PositionState
    {
        public Vector3 CurrentPosition { get; set; }
        public DateTime NextMoveRequest { get; set; }
        public Vector3 MoveToPosition { get; set; }
    }
}