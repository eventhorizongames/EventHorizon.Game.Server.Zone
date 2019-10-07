using System.Threading.Tasks;
using EventHorizon.Server.Core.External.Connection.Model;

namespace EventHorizon.Server.Core.External.Connection
{
    public static class CoreServerConnectionExtensions
    {
        public static async Task<RegisteredZoneDetails> RegisterZone(
            this CoreServerConnection connection,
            ZoneRegistrationDetails request
        )
        {
            return await connection.SendAction<RegisteredZoneDetails>(
                "RegisterZone",
                request
            );
        }
        public static async Task Ping(
            this CoreServerConnection connection
        )
        {
            await connection.SendAction(
                "Ping"
            );
        }
    }
}