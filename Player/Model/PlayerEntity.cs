using System.Numerics;
using EventHorizon.Game.Server.Zone.Entity.Model;

namespace EventHorizon.Game.Server.Zone.Player.Model
{
    public struct PlayerEntity : IObjectEntity
    {
        public static PlayerEntity NULL = default(PlayerEntity);

        public long Id { get; set; }
        public string PlayerId { get; set; }
        public PositionState Position { get; set; }
        public string ConnectionId { get; set; }
        public EntityType Type { get; set; }
        public object Data { get; set; }

        public PlayerEntity(string playerId, string connectionId, PositionState positionState)
        {
            this.PlayerId = playerId;
            this.Position = positionState;
            this.ConnectionId = connectionId;
            
            this.Id = -1;
            this.Type = EntityType.PLAYER;
            this.Data = new { };
        }
    }
}