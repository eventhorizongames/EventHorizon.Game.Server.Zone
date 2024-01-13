namespace EventHorizon.Server.Core.Connection.Model;

public struct ServiceDetails
{
    public string ApplicationVersion { get; }

    public ServiceDetails(
        string applicationVersion
    )
    {
        ApplicationVersion = applicationVersion;
    }
}
