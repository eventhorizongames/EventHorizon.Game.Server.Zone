using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.Core.Dynamic;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;

namespace EventHorizon.Game.Server.Zone.Agent.Model
{
    public struct AgentEntity : IObjectEntity
    {
        private static AgentEntity NULL = default(AgentEntity);
        public static AgentEntity CreateNotFound()
        {
            return default(AgentEntity);
        }

        private dynamic _data;
        private AgentData _typedData;

        public long Id { get; set; }
        public EntityType Type { get; set; }
        public PositionState Position { get; set; }
        public IList<string> TagList { get; set; }

        public string Name { get; set; }
        public float Speed { get; set; }
        public AgentAiDetails Ai { get; set; }
        public AgentData TypedData
        {
            get
            {
                if (_data == null)
                {
                    _data = new NullingExpandoObject();
                    _typedData = new AgentData(_data);
                }
                return _typedData;
            }
        }
        public dynamic Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new NullingExpandoObject();
                    _typedData = new AgentData(_data);
                }
                return _data;
            }
            set
            {
                _data = value ?? new NullingExpandoObject();
                _typedData = new AgentData(_data);
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