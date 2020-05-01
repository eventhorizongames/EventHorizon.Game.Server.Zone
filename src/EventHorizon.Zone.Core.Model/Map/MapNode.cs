namespace EventHorizon.Zone.Core.Model.Map
{
    using System.Collections.Generic;
    using System.Numerics;
    using EventHorizon.Zone.Core.Model.Structure;

    public struct MapNode : IOctreeEntity
    {
        public int Index { get; set; }
        public Vector3 Position { get; set; }
        public IDictionary<string, object> Info { get; set; }

        public MapNode(
            Vector3 position
        )
        {
            Index = -1;
            Position = position;
            Info = new Dictionary<string, object>();
        }
        
        public MapNode(
            int index
        )
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
            var castObj = (MapNode)obj;

            return Index.Equals(castObj.Index);
        }

        /// <summary>
        /// Required to override to work with Octree usage.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Index.GetHashCode();
        }

        public bool IsFound()
        {
            return Info != null;
        }
    }
}