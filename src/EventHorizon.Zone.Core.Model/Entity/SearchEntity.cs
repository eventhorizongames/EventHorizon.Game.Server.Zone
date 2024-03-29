namespace EventHorizon.Game.Server.Zone.Entity.Model;

using System.Collections.Generic;
using System.Numerics;

using EventHorizon.Zone.Core.Model.Structure;

public struct SearchEntity
    : IOctreeEntity
{
    public Vector3 Position { get; }
    public long EntityId { get; }
    public IList<string> TagList { get; }

    public SearchEntity(
        long entityId,
        Vector3 position,
        IList<string>? tagList
    )
    {
        EntityId = entityId;
        Position = position;
        TagList = tagList ?? new List<string>();
    }

    #region Generated object Overrides
    /// <summary>
    /// Required to override to work with Octree usage.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
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

    public static bool operator ==(SearchEntity left, SearchEntity right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SearchEntity left, SearchEntity right)
    {
        return !(left == right);
    }
    #endregion
}
