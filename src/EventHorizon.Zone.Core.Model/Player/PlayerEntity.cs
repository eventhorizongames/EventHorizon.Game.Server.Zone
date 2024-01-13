namespace EventHorizon.Zone.Core.Model.Player;

using System.Collections.Concurrent;
using System.Collections.Generic;

using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;

public struct PlayerEntity
    : IObjectEntity
{
    private static PlayerEntity NULL = default;

    public long Id { get; set; }
    public string PlayerId { get; set; }
    public string GlobalId
    {
        get
        {
            return PlayerId;
        }
    }
    public string Name { get; set; }
    public string Locale { get; set; }
    public EntityType Type { get; set; }
    public TransformState Transform { get; set; }
    public IList<string>? TagList { get; set; }
    public string ConnectionId { get; set; }

    private ConcurrentDictionary<string, object> _data;
    private ConcurrentDictionary<string, object> _rawData;
    public ConcurrentDictionary<string, object> RawData
    {
        get
        {
            return _rawData ?? new ConcurrentDictionary<string, object>();
        }
        set
        {
            _data = new ConcurrentDictionary<string, object>();
            _rawData = value;
        }
    }
    public ConcurrentDictionary<string, object> Data
    {
        get
        {
            if (_data == null)
            {
                _data = new ConcurrentDictionary<string, object>();
            }
            return _data;
        }
    }

    public bool IsFound()
    {
        return !Equals(NULL);
    }
}
