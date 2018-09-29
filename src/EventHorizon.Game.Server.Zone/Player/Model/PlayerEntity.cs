using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Player.Model
{
    public struct PlayerEntity : IObjectEntity
    {
        private static PlayerEntity NULL = default(PlayerEntity);

        public long Id { get; set; }
        public string PlayerId { get; set; }
        public EntityType Type { get; set; }
        public PositionState Position { get; set; }
        public IList<string> TagList { get; set; }
        public string ConnectionId { get; set; }
        public Dictionary<string, object> Data { get; set; }

        public T GetProperty<T>(string prop)
        {
            return (T)Data[prop];
        }

        public bool IsFound()
        {
            return !this.Equals(NULL);
        }
    }
}