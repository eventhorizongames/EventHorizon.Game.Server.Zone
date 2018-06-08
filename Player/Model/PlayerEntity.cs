using System.Numerics;

namespace EventHorizon.Game.Server.Zone.Player.Model
{
    public class PlayerEntity
    {
        public string Id { get; set; }
        public Vector3 Position { get; set; }
        public string ConnectionId { get; set; }
    }
}