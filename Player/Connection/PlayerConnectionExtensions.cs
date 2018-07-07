using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Connection;
using EventHorizon.Game.Server.Core.Player.Model;

namespace EventHorizon.Game.Server.Zone.Player.Connection
{
    public static class PlayerConnectionExtensions
    {
        public static async Task<IPlayerConnection> UpdatePlayer(this IPlayerConnection connection, PlayerDetails player)
        {
            await connection.SendAction("UpdatePlayer", player);
            return connection;
        }
        public static async Task<PlayerDetails> UpdatePlayer(this IPlayerConnection connection, string playerId)
        {
            return await connection.SendAction<PlayerDetails>("GetPlayer", playerId);
        }
    }
}