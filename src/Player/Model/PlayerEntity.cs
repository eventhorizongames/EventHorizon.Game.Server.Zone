using System.Numerics;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;

namespace EventHorizon.Game.Server.Zone.Player.Model
{
    public struct PlayerEntity : IObjectEntity
    {
        private static PlayerEntity NULL = default(PlayerEntity);

        public long Id { get; set; }
        public string PlayerId { get; set; }
        public PositionState Position { get; set; }
        public string ConnectionId { get; set; }
        public EntityType Type { get; set; }
        public object Data { get; set; }

        public bool IsFound()
        {
            return !this.Equals(NULL);
        }
    }
}