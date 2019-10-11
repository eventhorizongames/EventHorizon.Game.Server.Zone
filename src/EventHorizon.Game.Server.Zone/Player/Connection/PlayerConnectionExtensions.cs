using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Model;

namespace EventHorizon.Zone.System.Player.Connection
{
    public static class PlayerConnectionExtensions
    {
        public static async Task<PlayerServerConnection> UpdatePlayer(
            this PlayerServerConnection connection, 
            PlayerDetails player
        )
        {
            await connection.SendAction(
                "UpdatePlayer", 
                player
            );
            return connection;
        }
    }
}