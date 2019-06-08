using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Model.Structure;

namespace EventHorizon.Game.Server.Zone.Model.Map
{
    public struct MapNode : IOctreeEntity
    {
        public int Index { get; set; }
        public Vector3 Position { get; set; }
        public IDictionary<string, object> Info { get; set; }

        public MapNode(Vector3 position)
        {
            Index = -1;
            Position = position;
            Info = new Dictionary<string, object>();
        }
        public MapNode(int index)
        {
            Index = index;
            Position = new Vector3();
            Info = new Dictionary<string, object>();
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