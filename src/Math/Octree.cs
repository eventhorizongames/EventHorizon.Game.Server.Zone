using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace EventHorizon.Game.Server.Zone.Math
{
    public class Octree<T> where T : IOctreeEntity
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
            this.Accuracy = 0;
            this._root = new Cell<T>(accuracy, position, size, 0);
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
        public void GetAllCellsAtLevel(Cell<T> cell, int level, ref IList<Cell<T>> result)
        {
            if (cell.Level == level)
            {
                if (cell.Points.Count > 0)
                {
                    result.Add(cell);
                }
            }
            else
            {
                foreach (var child in cell.Children)
                {
                    this.GetAllCellsAtLevel(child, level, ref result);
                }
            }
        }
    }
    public interface IOctreeEntity
    {
        Vector3 Position { get; }
    }
    public class IOctreeOptions
    {
        public static readonly IOctreeOptions DEFAULT = new IOctreeOptions(float.MaxValue, false);
        public float MaxDist { get; }
        public bool NotSelf { get; }

        public IOctreeOptions(float maxDist, bool notSelf)
        {
            MaxDist = float.MaxValue;
            NotSelf = false;
        }
    }
}