using System.Numerics;

namespace EventHorizon.Zone.System.Player.Model.Position
{
    public class PlayerPositionState
    {
        public Vector3 Position { get; set; }
        public string CurrentZone { get; set; }
        public string ZoneTag { get; set; }
    }
}