namespace EventHorizon.Zone.Core.Model.Entity;

using System.Collections.Concurrent;
using System.Collections.Generic;

using EventHorizon.Zone.Core.Model.Core;

public struct DefaultEntity
    : IObjectEntity
{
    public static DefaultEntity NULL = default;
    public const string DEFAULT_GLOBAL_ID = "no_global_id";
    public long Id { get; set; }
    public string GlobalId
    {
        get
        {
            return DEFAULT_GLOBAL_ID;
        }
    }
    public string Name { get; set; }

    public EntityType Type { get { return EntityType.OTHER; } }

    public TransformState Transform { get; set; }
    public IList<string>? TagList { get; set; }

    private ConcurrentDictionary<string, object> _data;
    private ConcurrentDictionary<string, object> _rawData;

    public DefaultEntity(
        ConcurrentDictionary<string, object> rawData
    ) : this()
    {
        Id = -1L;
        Name = string.Empty;
        Transform = default;
        TagList = null;
        _data = new ConcurrentDictionary<string, object>();
        _rawData = rawData ?? new ConcurrentDictionary<string, object>();
    }

    public ConcurrentDictionary<string, object> RawData
    {
        get
        {
            return _rawData ??= new ConcurrentDictionary<string, object>();
        }
        set
        {
            if (_data == null)
                _data = new ConcurrentDictionary<string, object>();
            else
                _data.Clear();
            _rawData = value;
        }
    }
    public ConcurrentDictionary<string, object> Data
    {
        get
        {
            return _data ??= new ConcurrentDictionary<string, object>();
        }
    }

    public bool IsFound()
    {
        return !Equals(NULL);
    }

    #region Generated object Overrides
    public override bool Equals(object? obj)
    {
        if (obj == null
            || GetType() != obj.GetType()
            || obj is not IObjectEntity
        )
        {
            return false;
        }
        return Id.Equals(((IObjectEntity)obj).Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(DefaultEntity left, DefaultEntity right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(DefaultEntity left, DefaultEntity right)
    {
        return !(left == right);
    }
    #endregion
}
