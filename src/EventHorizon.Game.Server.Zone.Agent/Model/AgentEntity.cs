using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Model.Entity;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;

namespace EventHorizon.Game.Server.Zone.Agent.Model
{
    public struct AgentEntity : IObjectEntity
    {
        private static AgentEntity NULL = default(AgentEntity);
        public static AgentEntity CreateNotFound()
        {
            return default(AgentEntity);
        }

        public long Id { get; set; }
        public EntityType Type { get; set; }
        public PositionState Position { get; set; }
        public IList<string> TagList { get; set; }

        public string Name { get; set; }
        public float Speed { get; set; }

        private Dictionary<string, object> _data;
        private Dictionary<string, object> _rawData;
        public Dictionary<string, object> RawData
        {
            get
            {
                return _rawData ?? new Dictionary<string, object>();
            }
            set
            {
                _data = new Dictionary<string, object>();
                _rawData = value;
            }
        }
        public Dictionary<string, object> Data
        {
            get
            {
                return _data ?? new Dictionary<string, object>();
            }
        }

        // Volatile Entity Data
        public Queue<Vector3> Path { get; set; }

        public bool IsFound()
        {
            return !this.Equals(NULL);
        }
    }
}