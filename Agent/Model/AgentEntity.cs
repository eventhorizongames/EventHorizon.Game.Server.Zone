using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;

namespace EventHorizon.Game.Server.Zone.Agent.Model
{
    public struct AgentEntity : IObjectEntity
    {
        private static AgentEntity NULL = default(AgentEntity);

        private dynamic _data;

        public long Id { get; set; }
        public PositionState Position { get; set; }
        public EntityType Type { get; set; }

        public AgentData TypedData { get; private set; }
        public Queue<Vector3> Path { get; set; }
        public AgentAiDetails Ai { get; set; }
        public int Speed { get; set; }
        
        public dynamic Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                TypedData = new AgentData(_data);
            }
        }

        public bool IsFound()
        {
            return !this.Equals(NULL);
        }
    }
}