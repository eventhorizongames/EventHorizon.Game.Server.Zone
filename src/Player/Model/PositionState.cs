using System.Numerics;

namespace EventHorizon.Game.Server.Core.Player.Model
{
    public class PlayerPositionState
    {
        public Vector3 Position { get; set; }
        public string CurrentZone { get; set; }
        public string ZoneTag { get; set; }
    }
}