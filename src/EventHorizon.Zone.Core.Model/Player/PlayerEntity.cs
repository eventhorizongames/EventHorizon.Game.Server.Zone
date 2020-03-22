using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Zone.Core.Model.Player
{
    public struct PlayerEntity : IObjectEntity
    {
        private static PlayerEntity NULL = default(PlayerEntity);

        public long Id { get; set; }
        public string PlayerId { get; set; }
        public string GlobalId
        {
            get
            {
                return PlayerId;
            }
        }
        public string Name { get; set; }
        public string Locale { get; set; }
        public EntityType Type { get; set; }
        public TransformState Transform { get; set; }
        public IList<string> TagList { get; set; }
        public string ConnectionId { get; set; }

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
            return !this.Equals(NULL);
        }
    }
}