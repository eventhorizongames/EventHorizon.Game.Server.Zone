namespace EventHorizon.Server.Core.Connection.Internal
{
    using System.Threading.Tasks;
    using EventHorizon.Server.Core.Connection.Model;

    public class SystemCoreServerConnectionApi : CoreServerConnectionApi
    {
        private readonly CoreServerConnection _connection;

        public SystemCoreServerConnectionApi(
            CoreServerConnection connection
        )
        {
            _connection = connection;
        }

        public async Task<RegisteredZoneDetails> RegisterZone(
            ZoneRegistrationDetails request
        )
        {
            return await _connection.SendAction<RegisteredZoneDetails>(
                "RegisterZone",
                request
            );
        }

        public async Task Ping()
        {
            await _connection.SendAction(
                "Ping"
            );
        }
    }
}