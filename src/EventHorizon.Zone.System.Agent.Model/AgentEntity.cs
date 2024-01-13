namespace EventHorizon.Zone.System.Agent.Model;

using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;

using global::System.Collections.Concurrent;
using global::System.Collections.Generic;

public struct AgentEntity
    : IObjectEntity
{
    private static AgentEntity NULL = default;
    public static AgentEntity CreateNotFound()
    {
        return NULL;
    }

    private ConcurrentDictionary<string, object> _data;
    private ConcurrentDictionary<string, object> _rawData;

    public long Id { get; set; }
    public bool IsGlobal { get; set; }
    public string AgentId { get; set; }
    public string GlobalId
    {
        get
        {
            return AgentId;
        }
        set
        {
            AgentId = value;
        }
    }
    public EntityType Type { get; set; }

    public TransformState Transform { get; set; }
    public IList<string>? TagList { get; set; }

    public string Name { get; set; }
    public ConcurrentDictionary<string, object> RawData
    {
        get
        {
            return _rawData;
        }
        set
        {
            _data = new ConcurrentDictionary<string, object>();
            _rawData = value;
            if (_rawData == null)
            {
                _rawData = new ConcurrentDictionary<string, object>();
            }
        }
    }
    public ConcurrentDictionary<string, object> Data
    {
        get
        {
            return _data;
        }
    }

    public AgentEntity(
        ConcurrentDictionary<string, object>? rawData
    )
    {
        _data = new ConcurrentDictionary<string, object>();
        _rawData = rawData ?? new ConcurrentDictionary<string, object>();
        Id = -1L;
        IsGlobal = false;
        AgentId = string.Empty;
        Type = EntityType.AGENT;
        Transform = default;
        TagList = null;
        Name = string.Empty;
    }

    public bool IsFound()
    {
        return !Equals(NULL);
    }
}
