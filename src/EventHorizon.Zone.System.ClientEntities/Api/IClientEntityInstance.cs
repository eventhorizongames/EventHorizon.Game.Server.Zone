using System.Collections.Generic;
using System.Numerics;

namespace EventHorizon.Zone.System.ClientEntities.Api
{
    public interface IClientEntityInstance
    {
        string Id { get; }
        string Name { get; }
        Vector3 Position { get; }
        string AssetId { get; }
        IDictionary<string, object> Properties { get; }
    }
}