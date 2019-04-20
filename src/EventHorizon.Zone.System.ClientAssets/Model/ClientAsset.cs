using System.Collections.Generic;
using EventHorizon.Zone.System.ClientEntity.Api;

namespace EventHorizon.Zone.System.ClientAssets.Model
{
    public class ClientAsset : IClientAsset
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public IDictionary<string, object> Data { get; set; }
    }
}