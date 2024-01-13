namespace EventHorizon.Zone.System.Player.Connection;

using global::System.Threading.Tasks;

using EventHorizon.Zone.System.Player.Model.Details;

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
