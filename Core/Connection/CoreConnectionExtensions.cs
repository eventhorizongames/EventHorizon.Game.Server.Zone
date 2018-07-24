using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Model;

namespace EventHorizon.Game.Server.Zone.Core.Connection
{
    public static class CoreConnectionExtensions
    {
        public static async Task<RegisteredZoneDetails> RegisterZone(this ICoreConnection connection, ZoneRegistrationDetails request)
        {
            return await connection.SendAction<RegisteredZoneDetails>("RegisterZone", request);
        }
        public static async Task Ping(this ICoreConnection connection)
        {
            await connection.SendAction("Ping");
        }
    }
}