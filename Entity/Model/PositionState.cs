using System;
using System.Numerics;

namespace EventHorizon.Game.Server.Zone.Entity.Model
{
    public struct PositionState
    {
        public Vector3 CurrentPosition { get; set; }
        public DateTime NextMoveRequest { get; set; }
        public Vector3 MoveToPosition { get; set; }
    }
}