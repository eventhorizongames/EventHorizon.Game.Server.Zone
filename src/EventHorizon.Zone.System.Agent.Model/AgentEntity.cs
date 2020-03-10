using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Zone.System.Agent.Model
{
    public struct AgentEntity : IObjectEntity
    {
        private static AgentEntity NULL = default(AgentEntity);
        public static AgentEntity CreateNotFound()
        {
            return NULL;
        }

        private Dictionary<string, object> _data;
        private Dictionary<string, object> _rawData;

        public long Id { get; set; }
        public bool IsGlobal { get; set; }
        public string AgentId { get; set; }
        public string GlobalId
        {
            get
            {
                return this.AgentId;
            }
        }
        public EntityType Type { get; set; }

        public PositionState Position { get; set; }
        public IList<string> TagList { get; set; }

        public string Name { get; set; }
        public Dictionary<string, object> RawData
        {
            get
            {
                return _rawData;
            }
            set
            {
                _data = new Dictionary<string, object>();
                _rawData = value;
                if (_rawData == null)
                {
                    _rawData = new Dictionary<string, object>();
                }
            }
        }
        public Dictionary<string, object> Data
        {
            get
            {
                return _data;
            }
        }

        public AgentEntity(
            Dictionary<string, object> rawData
        )
        {
            _data = new Dictionary<string, object>();
            _rawData = rawData ?? new Dictionary<string, object>();
            Id = -1L;
            IsGlobal = false;
            AgentId = string.Empty;
            Type = EntityType.AGENT;
            Position = default(PositionState);
            TagList = null;
            Name = string.Empty;
        }

        public bool IsFound()
        {
            return !this.Equals(NULL);
        }
    }
}