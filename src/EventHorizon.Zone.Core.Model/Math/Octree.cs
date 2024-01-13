namespace EventHorizon.Zone.Core.Model.Math;

using System.Collections.Generic;
using System.Numerics;

using EventHorizon.Zone.Core.Model.Structure;

public class Octree<T>
    where T : struct, IOctreeEntity
{
    public static int MAX_LEVEL = 8;

    private Cell<T> _root;

    public int Accuracy { get; internal set; }

    public Octree(
        Vector3 position,
        Vector3 size,
        int accuracy
    )
    {
        _root = new Cell<T>(
            accuracy,
            position,
            size,
            accuracy
        );
        Reset(
            position,
            size,
            accuracy
        );
    }

    public void Reset(
        Vector3 position,
        Vector3 size,
        int accuracy
    )
    {
        Accuracy = accuracy;
        _root = new Cell<T>(
            accuracy,
            position,
            size,
            accuracy
        );
    }

    public void Add(
        T point
    )
    {
        _root.Add(
            point
        );
    }

    public void Remove(
        T point
    )
    {
        _root.Remove(
            point
        );
    }

    public bool Has(
        T point
    )
    {
        return _root.Has(
            point
        );
    }

    public List<T> All()
    {
        return _root.All(
            new List<T>()
        );
    }

    public T FindNearestPoint(
        Vector3 point,
        IOctreeOptions? options = null
    )
    {
        return _root.FindNearestPoint(
            point,
            options ?? IOctreeOptions.DEFAULT
        );
    }

    public IList<T> FindNearbyPoints(
        Vector3 position,
        float radius,
        IOctreeOptions? options = null
    )
    {
        var result = new List<T>();
        _root.FindNearbyPoints(
            position,
            radius,
            options ?? IOctreeOptions.DEFAULT,
            ref result
        );
        return result;
    }

    public IList<T> FindNearbyPoints(
        Vector3 position,
        Vector3 dimension,
        IOctreeOptions? options = null
    )
    {
        var result = new List<T>();
        _root.FindNearbyPointsInDimension(
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
    public static readonly IOctreeOptions DEFAULT = new(float.MaxValue, false);
    public float MaxDist { get; }
    public bool NotSelf { get; }

    public IOctreeOptions(
        float maxDist,
        bool notSelf
    )
    {
        MaxDist = maxDist;
        NotSelf = notSelf;
    }
}
