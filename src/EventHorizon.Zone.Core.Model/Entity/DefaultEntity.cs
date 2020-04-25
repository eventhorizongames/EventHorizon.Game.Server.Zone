namespace EventHorizon.Zone.Core.Model.Entity
{
    using EventHorizon.Zone.Core.Model.Core;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public struct DefaultEntity : IObjectEntity
    {
        public static DefaultEntity NULL = default(DefaultEntity);
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

        public TransformState Transform { get; set; }
        public IList<string> TagList { get; set; }

        private ConcurrentDictionary<string, object> _data;
        private ConcurrentDictionary<string, object> _rawData;

        public DefaultEntity(
            ConcurrentDictionary<string, object> rawData
        ) : this()
        {
            Id = -1L;
            Name = null;
            // GlobalId = null;
            Transform = default(TransformState);
            TagList = null;
            _data = new ConcurrentDictionary<string, object>();
            _rawData = rawData ?? new ConcurrentDictionary<string, object>();
        }

        public ConcurrentDictionary<string, object> RawData
        {
            get
            {
                return _rawData ?? (_rawData = new ConcurrentDictionary<string, object>());
            }
            set
            {
                if (_data == null)
                    _data = new ConcurrentDictionary<string, object>();
                else
                    _data.Clear();
                _rawData = value;
            }
        }
        public ConcurrentDictionary<string, object> Data
        {
            get
            {
                return _data ?? (_data = new ConcurrentDictionary<string, object>());
            }
        }

        public bool IsFound()
        {
            return !this.Equals(NULL);
        }

        public override bool Equals(object obj)
        {
            if (obj == null
                || GetType() != obj.GetType()
                || !(obj is IObjectEntity)
            )
            {
                return false;
            }
            return Id.Equals(((IObjectEntity)obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}