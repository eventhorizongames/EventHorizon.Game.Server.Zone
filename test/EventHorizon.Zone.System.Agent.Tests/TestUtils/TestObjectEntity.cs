using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Core;

namespace EventHorizon.TestUtils
{
    public struct TestObjectEntity : IObjectEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string GlobalId { get; set; }

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
                return _data;
            }
        }

        public TestObjectEntity(
            Dictionary<string, object> rawData
        )
        {
            Id = 0L;
            Name = null;
            GlobalId = null;
            Position = default(PositionState);
            TagList = null;
            _data = new Dictionary<string, object>();
            _rawData = rawData ?? new Dictionary<string, object>();
        }

        public bool IsFound()
        {
            return !this.Equals(default(TestObjectEntity));
        }
    }
}