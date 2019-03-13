using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Core;

namespace EventHorizon.Game.Server.Zone.Model.Entity
{
    public struct DefaultEntity : IObjectEntity
    {
        public const string DEFAULT_GLOBAL_ID = "no_global_id";
        public long Id { get; set; }
        public string GlobalId
        {
            get
            {
                return DefaultEntity.DEFAULT_GLOBAL_ID;
            }
        }
        public string Name { get; set; }

        public EntityType Type { get { return EntityType.OTHER; } }

        public PositionState Position { get; set; }
        public IList<string> TagList { get; set; }

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

        public bool IsFound()
        {
            return false;
        }
    }
}