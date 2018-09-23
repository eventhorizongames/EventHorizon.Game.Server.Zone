using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System;
using System.Collections.Concurrent;
using EventHorizon.Game.Server.Zone.Model.Structure;

namespace EventHorizon.Game.Server.Zone.Math
{
    /// <summary>
    ///     
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Cell<T> where T : struct, IOctreeEntity
    {
        private static readonly object UPDATE_LOCK = new object();
        private static int ALLOWED_POINTS = 8;
        public IDictionary<long, T> Points { get; }
        public IList<Cell<T>> Children { get; }
        public ConcurrentDictionary<long, T> Search_Points { get; private set; }
        public ConcurrentBag<Cell<T>> Search_Children { get; private set; }
        public Vector3 Position { get; }
        public Vector3 Size { get; }
        public int Level { get; }

        private int _accuracy;

        public Cell(int accuracy, Vector3 position, Vector3 size, int level)
        {
            _accuracy = accuracy;
            Position = position;
            Size = size;
            Level = level;

            Points = new Dictionary<long, T>();
            Children = new List<Cell<T>>();

            Search_Points = new ConcurrentDictionary<long, T>();
            Search_Children = new ConcurrentBag<Cell<T>>();
        }

        public bool Has(T point)
        {
            if (!this.Contains(point))
            {
                return false;
            }
            foreach (var child in Search_Children)
            {
                if (child.Has(point))
                {
                    return true;
                }
            }
            var minDistSqrt = this._accuracy * this._accuracy;
            foreach (var otherPoint in Search_Points)
            {
                var distSq = Vector3.DistanceSquared(otherPoint.Value.Position, point.Position);
                if (distSq <= minDistSqrt)
                {
                    return true;
                }
            }
            return false;
        }

        public List<T> All(List<T> list)
        {
            list.AddRange(Search_Points.Select(a => a.Value));
            if (Children.Count > 0)
            {
                foreach (var child in Search_Children)
                {
                    list = child.All(list);
                }
            }
            return list;
        }

        public bool Contains(T point)
        {
            return point.Position.X >= this.Position.X - this._accuracy
                && point.Position.Y >= this.Position.Y - this._accuracy
                && point.Position.Z >= this.Position.Z - this._accuracy
                && point.Position.X < this.Position.X + this.Size.X + this._accuracy
                && point.Position.Y < this.Position.Y + this.Size.Y + this._accuracy
                && point.Position.Z < this.Position.Z + this.Size.Z + this._accuracy;
        }
        public void Add(T point)
        {
            lock (UPDATE_LOCK)
            {
                // TODO: see if we can remove points from cell when moving into Children.
                if (this.Children.Count > 0)
                {
                    this.AddToChildren(point);
                }
                else
                {
                    this.Points.Remove(point.GetHashCode());
                    this.Points.Add(point.GetHashCode(), point);
                    if (this.Points.Count > ALLOWED_POINTS && this.Level < Octree<T>.MAX_LEVEL)
                    {
                        this.Split();
                    }
                }
                this.Search_Points = new ConcurrentDictionary<long, T>(this.Points);
                this.Search_Children = new ConcurrentBag<Cell<T>>(this.Children);
            }
        }
        public bool Remove(T pointToRemove)
        {
            lock (UPDATE_LOCK)
            {
                var removed = false;
                foreach (var point in Points)
                {
                    if (point.Value.Equals(pointToRemove))
                    {
                        var removedValue = default(T);
                        removed = Points.Remove(point.Value.GetHashCode(), out removedValue);
                        break;
                    }
                }
                if (!removed)
                {
                    foreach (var child in Children)
                    {
                        removed = child.Remove(pointToRemove);
                        if (removed)
                        {
                            break;
                        }
                    }
                }
                if (removed && Children.Count > 0)
                {
                    if (ShouldMerge())
                    {
                        Merge();
                    }
                }
                this.Search_Points = new ConcurrentDictionary<long, T>(this.Points);
                this.Search_Children = new ConcurrentBag<Cell<T>>(this.Children);
                return removed;
            }
        }

        private bool ShouldMerge()
        {
            var totalPoints = Points.Count;
            foreach (var child in Children)
            {
                if (child.Children.Count > 0)
                {
                    return false;
                }
                totalPoints += child.Points.Count;
            }
            return totalPoints <= ALLOWED_POINTS;
        }
        private void Merge()
        {
            foreach (var child in Children)
            {
                foreach (var point in child.Points)
                {
                    this.Points.Add(point.Value.GetHashCode(), point.Value);
                }
            }
            this.Children.Clear();
        }
        private void AddToChildren(T point)
        {
            foreach (var child in Children)
            {
                if (child.Contains(point))
                {
                    child.Add(point);
                    break;
                }
            }
        }
        private void Split()
        {
            var x = this.Position.X;
            var y = this.Position.Y;
            var z = this.Position.Z;
            var w2 = this.Size.X / 2;
            var h2 = this.Size.Y / 2;
            var d2 = this.Size.Z / 2;
            this.Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x, y, z),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x + w2, y, z),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x, y, z + d2),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x + w2, y, z + d2),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x, y + h2, z),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x + w2, y + h2, z),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x, y + h2, z + d2),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x + w2, y + h2, z + d2),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            foreach (var point in Points)
            {
                this.AddToChildren(point.Value);
            }
            this.Points.Clear();
        }


        private float SquareDistanceToCenter(Vector3 point)
        {
            var dx = point.X - (this.Position.X + this.Size.X / 2);
            var dy = point.Y - (this.Position.Y + this.Size.Y / 2);
            var dz = point.Z - (this.Position.Z + this.Size.Z / 2);
            return dx * dx + dy * dy + dz * dz;
        }

        public T FindNearestPoint(Vector3 position, IOctreeOptions options)
        {
            T nearest = default(T);
            var bestDist = options.MaxDist;

            if (this.Search_Points.Count > 0 && this.Search_Children.Count == 0)
            {
                foreach (var point in Search_Points)
                {
                    var dist = Vector3.Distance(position, point.Value.Position); // TODO: Might need to flip this.
                    if (dist <= bestDist)
                    {
                        if (dist == 0 && options.NotSelf)
                        {
                            continue;
                        }
                        bestDist = dist;
                        nearest = point.Value;
                    }
                }
            }

            var children = this.Search_Children
                .Select((child) => new
                {
                    child = child,
                    dist = child.SquareDistanceToCenter(position),
                })
                .OrderBy(a => a.dist)
                .Select((c) => c.child);

            foreach (var child in children)
            {
                if (position.X < child.Position.X - bestDist
                    || position.X > child.Position.X + child.Size.X + bestDist
                    || position.Y < child.Position.Y - bestDist
                    || position.Y > child.Position.Y + child.Size.Y + bestDist
                    || position.Z < child.Position.Z - bestDist
                    || position.Z > child.Position.Z + child.Size.Z + bestDist
                )
                {
                    continue;
                }
                var childNearest = child.FindNearestPoint(position, options);
                if (childNearest.Equals(default(T)))
                {
                    continue;
                }
                var childNearestDist = Vector3.Distance(childNearest.Position, position);
                if (childNearestDist < bestDist)
                {
                    nearest = childNearest;
                    bestDist = childNearestDist;
                }
            }
            return nearest;
        }

        public void FindNearbyPoints(Vector3 position, float radius, IOctreeOptions options, ref List<T> result)
        {
            if (this.Search_Points.Count > 0 && this.Search_Children.Count == 0)
            {
                foreach (var point in Search_Points)
                {
                    var dist = Vector3.Distance(position, point.Value.Position); // TODO: Might need to flip this
                    if (dist <= radius)
                    {
                        if (dist == 0 && options.NotSelf)
                        {
                            continue;
                        }
                        result.Add(point.Value);
                    }
                }
            }
            foreach (var child in this.Search_Children)
            {
                child.FindNearbyPoints(position, radius, options, ref result);
            }
        }
    }
}