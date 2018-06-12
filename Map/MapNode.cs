using System.Numerics;
using EventHorizon.Game.Server.Zone.Math;

namespace EventHorizon.Game.Server.Zone.Map
{
    public struct MapNode : IOctreeEntity
    {
        public int Index { get; set; }
        public Vector3 Position { get; set; }
        public dynamic Info { get; set; }

        public MapNode(Vector3 position) {
            Index = -1;
            Position = position;
            Info = new {};
        }
    }
}