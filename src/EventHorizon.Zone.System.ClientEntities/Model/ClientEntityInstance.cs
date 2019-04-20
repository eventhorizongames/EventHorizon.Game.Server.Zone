using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Zone.System.ClientEntities.Api;

namespace EventHorizon.Zone.System.ClientEntities.Model
{
    public class ClientEntityInstance : IClientEntityInstance
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public string AssetId { get; set; }
        public IDictionary<string, object> Properties { get; set; }
    }
}