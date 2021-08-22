namespace EventHorizon.Zone.Core.Model.Math
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    using EventHorizon.Zone.Core.Model.Structure;

    /// <summary>
    ///     
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Cell<T> where T : struct, IOctreeEntity
    {
        private static readonly int ALLOWED_POINTS = 8;

        public IDictionary<long, T> Points { get; }
        public IList<Cell<T>> Children { get; }
        public ConcurrentDictionary<long, T> Search_Points { get; private set; }
        public ConcurrentBag<Cell<T>> Search_Children { get; private set; }
        public Vector3 Position { get; }
        public Vector3 Size { get; }
        public int Level { get; }

        private readonly int _accuracy;
        private readonly object _updateLock = new();

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
            if (!Contains(point))
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
            var minDistSqrt = _accuracy * _accuracy;
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
            return point.Position.X >= Position.X - _accuracy
                && point.Position.Y >= Position.Y - _accuracy
                && point.Position.Z >= Position.Z - _accuracy
                && point.Position.X < Position.X + Size.X + _accuracy
                && point.Position.Y < Position.Y + Size.Y + _accuracy
                && point.Position.Z < Position.Z + Size.Z + _accuracy;
        }
        public void Add(T point)
        {
            lock (_updateLock)
            {
                if (Children.Count > 0)
                {
                    AddToChildren(point);
                }
                else
                {
                    Points.Remove(point.GetHashCode());
                    Points.Add(point.GetHashCode(), point);
                    if (Points.Count > ALLOWED_POINTS && Level < Octree<T>.MAX_LEVEL)
                    {
                        Split();
                    }
                }
                Search_Points = new ConcurrentDictionary<long, T>(Points);
                Search_Children = new ConcurrentBag<Cell<T>>(Children);
            }
        }
        public bool Remove(T pointToRemove)
        {
            lock (_updateLock)
            {
                var removed = false;
                foreach (var point in Points)
                {
                    if (point.Value.Equals(pointToRemove))
                    {
                        removed = Points.Remove(point.Value.GetHashCode());
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
                Search_Points = new ConcurrentDictionary<long, T>(Points);
                Search_Children = new ConcurrentBag<Cell<T>>(Children);
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
                    Points.Add(point.Value.GetHashCode(), point.Value);
                }
            }
            Children.Clear();
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
            var x = Position.X;
            var y = Position.Y;
            var z = Position.Z;
            var w2 = Size.X / 2;
            var h2 = Size.Y / 2;
            var d2 = Size.Z / 2;
            Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x, y, z),
                    new Vector3(w2, h2, d2),
                    Level + 1
                )
            );
            Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x + w2, y, z),
                    new Vector3(w2, h2, d2),
                    Level + 1
                )
            );
            Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x, y, z + d2),
                    new Vector3(w2, h2, d2),
                    Level + 1
                )
            );
            Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x + w2, y, z + d2),
                    new Vector3(w2, h2, d2),
                    Level + 1
                )
            );
            Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x, y + h2, z),
                    new Vector3(w2, h2, d2),
                    Level + 1
                )
            );
            Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x + w2, y + h2, z),
                    new Vector3(w2, h2, d2),
                    Level + 1
                )
            );
            Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x, y + h2, z + d2),
                    new Vector3(w2, h2, d2),
                    Level + 1
                )
            );
            Children.Add(
                new Cell<T>(
                    _accuracy,
                    new Vector3(x + w2, y + h2, z + d2),
                    new Vector3(w2, h2, d2),
                    Level + 1
                )
            );
            foreach (var point in Points)
            {
                AddToChildren(point.Value);
            }
            Points.Clear();
        }


        private float SquareDistanceToCenter(Vector3 point)
        {
            var dx = point.X - (Position.X + Size.X / 2);
            var dy = point.Y - (Position.Y + Size.Y / 2);
            var dz = point.Z - (Position.Z + Size.Z / 2);
            return dx * dx + dy * dy + dz * dz;
        }

        public T FindNearestPoint(Vector3 position, IOctreeOptions options)
        {
            T nearest = default;
            var bestDist = options.MaxDist;

            if (Search_Points.Count > 0 && Search_Children.Count == 0)
            {
                foreach (var point in Search_Points)
                {
                    var dist = Vector3.Distance(position, point.Value.Position);
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

            var children = Search_Children
                .Select((child) => new
                {
                    child,
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
            if (Search_Points.Count > 0 && Search_Children.Count == 0)
            {
                foreach (var point in Search_Points)
                {
                    var dist = Vector3.Distance(position, point.Value.Position);
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
            foreach (var child in Search_Children)
            {
                child.FindNearbyPoints(position, radius, options, ref result);
            }
        }
        public void FindNearbyPointsInDimension(
            Vector3 position,
            Vector3 dimension,
            IOctreeOptions options,
            ref List<T> result
        )
        {
            if (Search_Points.Count > 0 && Search_Children.Count == 0)
            {
                foreach (var point in Search_Points)
                {
                    var rect1 = new
                    {
                        x = position.X - (dimension.X / 2),
                        y = position.Y - (dimension.Y / 2),
                        z = position.Z - (dimension.Z / 2),

                        width = dimension.X,
                        height = dimension.Y,
                        depth = dimension.Z,
                    };
                    var rect2 = new
                    {
                        x = point.Value.Position.X - 0.5,
                        y = point.Value.Position.Y - 0.5,
                        z = point.Value.Position.Z - 0.5,

                        width = 1,
                        height = 1,
                        depth = 1,
                    };
                    var dist = Vector3.Distance(position, point.Value.Position);
                    if (rect1.x < rect2.x + rect2.width &&
                        rect1.x + rect1.width > rect2.x &&
                        rect1.z < rect2.z + rect2.depth &&
                        rect1.z + rect1.depth > rect2.z
                    )
                    {
                        if (dist == 0 && options.NotSelf)
                        {
                            continue;
                        }
                        result.Add(point.Value);
                    }
                }
            }
            foreach (var child in Search_Children)
            {
                child.FindNearbyPointsInDimension(
                    position,
                    dimension,
                    options,
                    ref result
                );
            }
        }
    }
}
