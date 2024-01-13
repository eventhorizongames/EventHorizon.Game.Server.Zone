namespace EventHorizon.Zone.System.Client.Scripts.Model.Query;

public struct ClientScriptsAssemblyDetails
{
    public string Hash { get; }

    public ClientScriptsAssemblyDetails(
        string hash
    )
    {
        Hash = hash;
    }
}
