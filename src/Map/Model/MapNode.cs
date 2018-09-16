using System.Numerics;
using EventHorizon.Game.Server.Zone.Math;

namespace EventHorizon.Game.Server.Zone.Map.Model
{
    public struct MapNode : IOctreeEntity
    {
        public int Index { get; set; }
        public Vector3 Position { get; set; }
        public dynamic Info { get; set; }

        public MapNode(Vector3 position)
        {
            Index = -1;
            Position = position;
            Info = new { };
        }

        /// <summary>
        /// Required to override to work with Octree usage.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Index.Equals(((MapNode)obj).Index);
        }

        /// <summary>
        /// Required to override to work with Octree usage.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Index.GetHashCode();
        }
    }
}