using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace EventHorizon.Game.Server.Zone.Math
{
    public class Cell<T> where T : IOctreeEntity
    {
        public int Level { get; }
        public IList<T> Points { get; }
        public IList<Cell<T>> Children { get; }

        Vector3 Position { get; }
        Octree<T> _tree;
        Vector3 Size { get; }

        public Cell(Octree<T> tree, Vector3 position, Vector3 size, int level)
        {
            _tree = tree;
            Position = position;
            Size = size;
            Level = level;

            Points = new List<T>();
            Children = new List<Cell<T>>();
        }

        public bool Has(T point)
        {
            if (!this.Contains(point))
            {
                return false;
            }
            if (this.Children.Count > 0)
            {
                for (var i = 0; i < this.Children.Count; i++)
                {
                    var duplicate = this.Children[i].Has(point);
                    if (duplicate)
                    {
                        return duplicate;
                    }
                }
                return false;
            }
            else
            {
                var minDistSqrt = this._tree.Accuracy * this._tree.Accuracy;
                for (var i = 0; i < this.Points.Count; i++)
                {
                    var o = this.Points[i];
                    var distSq = Vector3.DistanceSquared(o.Position, point.Position);
                    if (distSq <= minDistSqrt)
                    {
                        return o != null;
                    }
                }
                return false;
            }
        }
        public bool Contains(T point)
        {
            return point.Position.X >= this.Position.X - this._tree.Accuracy
                && point.Position.Y >= this.Position.Y - this._tree.Accuracy
                && point.Position.Z >= this.Position.Z - this._tree.Accuracy
                && point.Position.X < this.Position.X + this.Size.X + this._tree.Accuracy
                && point.Position.Y < this.Position.Y + this.Size.Y + this._tree.Accuracy
                && point.Position.Z < this.Position.Z + this.Size.Z + this._tree.Accuracy;
        }
        public void Add(T point)
        {
            this.Points.Add(point);
            if (this.Children.Count > 0)
            {
                this.AddToChildren(point);
            }
            else
            {
                if (this.Points.Count > 1 && this.Level < Octree<T>.MAX_LEVEL)
                {
                    this.Split();
                }
            }
        }
        public void AddToChildren(T point)
        {
            for (var i = 0; i < this.Children.Count; i++)
            {
                if (this.Children[i].Contains(point))
                {
                    this.Children[i].Add(point);
                    break;
                }
            }
        }
        public void Split()
        {
            var x = this.Position.X;
            var y = this.Position.Y;
            var z = this.Position.Z;
            var w2 = this.Size.X / 2;
            var h2 = this.Size.Y / 2;
            var d2 = this.Size.Z / 2;
            this.Children.Add(
                new Cell<T>(
                    this._tree,
                    new Vector3(x, y, z),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    this._tree,
                    new Vector3(x + w2, y, z),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    this._tree,
                    new Vector3(x, y, z + d2),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    this._tree,
                    new Vector3(x + w2, y, z + d2),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    this._tree,
                    new Vector3(x, y + h2, z),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    this._tree,
                    new Vector3(x + w2, y + h2, z),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    this._tree,
                    new Vector3(x, y + h2, z + d2),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            this.Children.Add(
                new Cell<T>(
                    this._tree,
                    new Vector3(x + w2, y + h2, z + d2),
                    new Vector3(w2, h2, d2),
                    this.Level + 1
                )
            );
            for (var i = 0; i < this.Points.Count; i++)
            {
                this.AddToChildren(this.Points[i]);
            }
        }


        public float SquareDistanceToCenter(Vector3 point)
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

            if (this.Points.Count > 0 && this.Children.Count == 0)
            {
                for (var i = 0; i < this.Points.Count; i++)
                {
                    var dist = Vector3.Distance(position, this.Points[i].Position); // TODO: Might need to flip this.
                    if (dist <= bestDist)
                    {
                        if (dist == 0 && options.NotSelf)
                        {
                            continue;
                        }
                        bestDist = dist;
                        nearest = this.Points[i];
                    }
                }
            }

            var children = this.Children
                .Select((child) => new
                {
                    child = child,
                    dist = child.SquareDistanceToCenter(position),
                })
                .OrderBy(a => a.dist)
                .Select((c) => c.child);

            foreach (var child in children)
            {
                if (child.Points.Count > 0)
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
                    if (childNearest == null)
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
            }
            return nearest;
        }

        public void FindNearbyPoints(Vector3 position, float radius, IOctreeOptions options, ref List<T> result)
        {
            if (this.Points.Count > 0 && this.Children.Count == 0)
            {
                for (var i = 0; i < this.Points.Count; i++)
                {
                    var dist = Vector3.Distance(position, this.Points[i].Position); // TODO: Might need to flip this
                    if (dist <= radius)
                    {
                        if (dist == 0 && options.NotSelf)
                        {
                            continue;
                        }
                        result.Add(this.Points[i]);
                    }
                }
            }
            foreach (var child in this.Children)
            {
                if (child.Points.Count > 0)
                {
                    if (position.X < child.Position.X - radius
                        || position.X > child.Position.X + child.Size.X + radius
                        || position.Y < child.Position.Y - radius
                        || position.Y > child.Position.Y + child.Size.Y + radius
                        || position.Z < child.Position.Z - radius
                        || position.Z > child.Position.Z + child.Size.Z + radius
                    )
                    {
                        continue;
                    }
                    child.FindNearbyPoints(position, radius, options, ref result);
                }
            }
        }
    }
}