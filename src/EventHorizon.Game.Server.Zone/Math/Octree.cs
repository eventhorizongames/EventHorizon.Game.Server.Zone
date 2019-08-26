using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Model.Structure;

namespace EventHorizon.Game.Server.Zone.Math
{
    public class Octree<T> where T : struct, IOctreeEntity
    {
        public static int MAX_LEVEL = 8;
        private float _maxDistance;
        private Cell<T> _root;
        public int Accuracy { get; internal set; } = 0;

        public Octree(Vector3 position, Vector3 size, int accuracy)
        {
            this.Reset(position, size, accuracy);
        }

        public void Reset(Vector3 position, Vector3 size, int accuracy)
        {
            this._maxDistance = System.Math.Max(size.X, System.Math.Max(size.Y, size.Z));
            this.Accuracy = accuracy;
            this._root = new Cell<T>(accuracy, position, size, accuracy);
        }

        public void Add(T point)
        {
            this._root.Add(point);
        }
        public void Remove(T point)
        {
            this._root.Remove(point);
        }
        public bool Has(T point)
        {
            return this._root.Has(point);
        }
        public List<T> All()
        {
            return this._root.All(new List<T>());
        }
        public T FindNearestPoint(Vector3 p, IOctreeOptions options = null)
        {
            return this._root.FindNearestPoint(p, options ?? IOctreeOptions.DEFAULT);
        }
        public IList<T> FindNearbyPoints(Vector3 position, float radius, IOctreeOptions options = null)
        {
            var result = new List<T>();
            this._root.FindNearbyPoints(position, radius, options ?? IOctreeOptions.DEFAULT, ref result);
            return result;
        }
        public IList<T> FindNearbyPoints(
            Vector3 position,
            Vector3 dimension,
            IOctreeOptions options = null
        )
        {
            var result = new List<T>();
            this._root.FindNearbyPointsInDimension(
                position,
                dimension,
                options ?? IOctreeOptions.DEFAULT,
                ref result
            );
            return result;
        }
    }
    public class IOctreeOptions
    {
        public static readonly IOctreeOptions DEFAULT = new IOctreeOptions(float.MaxValue, false);
        public float MaxDist { get; }
        public bool NotSelf { get; }

        public IOctreeOptions(float maxDist, bool notSelf)
        {
            MaxDist = float.MaxValue;
            NotSelf = notSelf;
        }
    }
}