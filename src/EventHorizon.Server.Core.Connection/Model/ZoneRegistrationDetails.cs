namespace EventHorizon.Server.Core.Connection.Model;

public struct ZoneRegistrationDetails
{
    public string ServerAddress { get; }
    public string Tag { get; }
    public ServiceDetails Details { get; }

    public ZoneRegistrationDetails(
        string serverAddress,
        string tag,
        ServiceDetails details
    )
    {
        ServerAddress = serverAddress;
        Tag = tag;
        Details = details;
    }
}
