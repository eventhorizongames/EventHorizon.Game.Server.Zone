using System.Numerics;
using EventHorizon.Game.Server.Zone.Math;

namespace EventHorizon.Game.Server.Zone.Entity.Model
{
    public struct SearchEntity : IOctreeEntity
    {
        public Vector3 Position { get; }
        public long EntityId { get; }

        public SearchEntity(long entityId, Vector3 position)
        {
            EntityId = entityId;
            Position = position;
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

            return EntityId.Equals(((SearchEntity)obj).EntityId);
        }

        /// <summary>
        /// Required to override to work with Octree usage.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return EntityId.GetHashCode();
        }
    }
}