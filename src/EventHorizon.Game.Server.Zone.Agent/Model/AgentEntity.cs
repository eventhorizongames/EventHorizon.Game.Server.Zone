using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Model.Entity;
using Newtonsoft.Json.Linq;
using System.Linq;

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
        private Dictionary<string, object> _tempData;

        public long Id { get; set; }
        public EntityType Type { get; set; }
        public PositionState Position { get; set; }
        public IList<string> TagList { get; set; }

        public string Name { get; set; }
        public float Speed { get; set; }

        // Volatile Entity Data
        public Queue<Vector3> Path { get; set; }

        public bool IsFound()
        {
            return !this.Equals(NULL);
        }


        public Dictionary<string, object> Data
        {
            get
            {
                var data = new Dictionary<string, object>();
                foreach (var prop in _tempData)
                {
                    data[prop.Key] = prop.Value;
                }
                foreach (var prop in _data)
                {
                    data[prop.Key] = prop.Value;
                }
                return data;
            }
            set
            {
                _data = new Dictionary<string, object>();
                _tempData = value;
            }
        }

        public T GetProperty<T>(string prop)
        {
            object value = default(T);
            _data.TryGetValue(prop, out value);
            if (value == null)
            {
                return default(T);
            }
            return (T)value;
        }

        public void SetProperty<T>(string prop, T value)
        {
            _data[prop] = value;
        }
        /// <summary>
        /// TODO: Add default value to be used when temp data is not found.
        /// </summary>
        /// <param name="prop"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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