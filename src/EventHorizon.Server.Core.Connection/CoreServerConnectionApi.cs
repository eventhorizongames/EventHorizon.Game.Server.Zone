namespace EventHorizon.Server.Core.Connection
{
    using System.Threading.Tasks;

    using EventHorizon.Server.Core.Connection.Model;

    public interface CoreServerConnectionApi
    {
        Task<RegisteredZoneDetails> RegisterZone(
            ZoneRegistrationDetails request
        );

        Task Ping();
    }
}
