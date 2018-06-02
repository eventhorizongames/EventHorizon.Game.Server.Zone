using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace EventHorizon.Game.Server.Zone.Math
{
    public class Octree<T> where T : IOctreeEntity
    {
        public static int MAX_LEVEL = 8;
        private float _maxDistance;
        private Cell<T> root;
        public int Accuracy { get; internal set; } = 0;

        public Octree(Vector3 position, Vector3 size, int accuracy)
        {
            this._maxDistance = System.Math.Max(size.X, System.Math.Max(size.Y, size.Z));
            this.Accuracy = 0;
            this.root = new Cell<T>(this, position, size, 0);
        }

        public void Reset(Vector3 position, Vector3 size, int accuracy)
        {
            this._maxDistance = System.Math.Max(size.X, System.Math.Max(size.Y, size.Z));
            this.Accuracy = 0;
            this.root = new Cell<T>(this, position, size, 0);
        }

        public void Add(T point)
        {
            this.root.Add(point);
        }
        public bool Has(T point)
        {
            return this.root.Has(point);
        }
        public T FindNearestPoint(Vector3 p, IOctreeOptions options)
        {
            return this.root.FindNearestPoint(p, options);
        }
        public IList<T> FindNearbyPoints(Vector3 position, float radius, IOctreeOptions options)
        {
            if (options == null)
            {
                options = new IOctreeOptions();
            }
            var result = new List<T>();
            this.root.FindNearbyPoints(position, radius, options, ref result);
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
        public float MaxDist { get; internal set; } = float.MaxValue;
        public bool NotSelf { get; internal set; } = false;
    }
}