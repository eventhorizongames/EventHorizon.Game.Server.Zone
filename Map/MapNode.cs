using System.Numerics;
using EventHorizon.Game.Server.Zone.Math;

namespace EventHorizon.Game.Server.Zone.Map
{
    public class MapNode : IOctreeEntity
    {
        public int Index { get; set; } = -1;
        public Vector3 Position { get; set; }
        public dynamic Info { get; set; } = new { };
    }
}