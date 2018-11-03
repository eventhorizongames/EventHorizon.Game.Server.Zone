using System;
using System.Numerics;

namespace EventHorizon.Game.Server.Zone.Model.Core
{
    public struct PositionState
    {
        public bool CanMove { get; set; }
        public Vector3 CurrentPosition { get; set; }
        public DateTime NextMoveRequest { get; set; }
        public Vector3 MoveToPosition { get; set; }
        public string CurrentZone { get; set; }
        public string ZoneTag { get; set; }
    }
}