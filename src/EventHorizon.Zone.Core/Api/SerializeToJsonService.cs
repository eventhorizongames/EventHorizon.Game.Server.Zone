namespace EventHorizon.Zone.Core.Api;

public interface SerializeToJsonService
{
    string Serialize(
        object objectToSerialize
    );
}
