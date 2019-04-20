using System.Collections.Generic;

namespace EventHorizon.Zone.System.ClientEntity.Api
{
    public interface IClientAsset
    {
        string Id { get; }
        string Type { get; }
        string Name { get; }
        IDictionary<string, object> Data { get; }
    }
}