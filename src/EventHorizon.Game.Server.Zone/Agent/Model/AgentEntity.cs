using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Model.Entity;
using Newtonsoft.Json.Linq;

namespace EventHorizon.Game.Server.Zone.Agent.Model
{
    public struct AgentEntity : IObjectEntity
    {
        private static AgentEntity NULL = default(AgentEntity);
        public static AgentEntity CreateNotFound()
        {
            return default(AgentEntity);
        }

        private Dictionary<string, object> _data;
        private dynamic _tempData;

        public long Id { get; set; }
        public EntityType Type { get; set; }
        public PositionState Position { get; set; }
        public IList<string> TagList { get; set; }

        public string Name { get; set; }
        public float Speed { get; set; }
        public dynamic Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new Dictionary<string, object>();
                }
                return _data;
            }
            set
            {
                _data = new Dictionary<string, object>();
                _tempData = value;
            }
        }

        // Volatile Entity Data
        public Queue<Vector3> Path { get; set; }

        public bool IsFound()
        {
            return !this.Equals(NULL);
        }

        public T GetProperty<T>(string prop)
        {
            object value = default(T);
            _data.TryGetValue(prop, out value);
            return (T)value;
        }

        public void SetProperty<T>(string prop, T value)
        {
            Data[prop] = value;
        }
        public T PopulateFromTempData<T>(string prop)
        {
            if (_tempData[prop] is JObject)
            {
                var tempProp = (JObject)_tempData[prop];
                _data.Add(prop, tempProp.ToObject<T>());
                return GetProperty<T>(prop);
            }
            if (_tempData[prop] is JToken)
            {
                var tempProp = (JToken)_tempData[prop];
                _data.Add(prop, tempProp.ToObject<T>());
                return GetProperty<T>(prop);
            }
            if (_tempData[prop] is T)
            {
                _data.Add(prop, _tempData[prop]);
                return GetProperty<T>(prop);
            }
            return default(T);
        }
    }
}