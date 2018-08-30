using System;
using System.Numerics;

namespace EventHorizon.Game.Server.Zone.Core.Model
{
    public struct PositionState
    {
        public Vector3 CurrentPosition { get; set; }
        public DateTime NextMoveRequest { get; set; }
        public Vector3 MoveToPosition { get; set; }
        public string CurrentZone { get; set; }
        public string ZoneTag { get; set; }
    }
}