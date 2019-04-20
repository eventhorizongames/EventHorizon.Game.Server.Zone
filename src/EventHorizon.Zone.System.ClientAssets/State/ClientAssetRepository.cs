using System.Collections.Concurrent;
using System.Collections.Generic;
using EventHorizon.Zone.System.ClientEntity.Api;

namespace EventHorizon.Zone.System.ClientAssets.State
{
    public interface ClientAssetRepository
    {
        void Add(IClientAsset clientAsset);
        IEnumerable<IClientAsset> All();
    }

    public class ClientAssetInMemoryRepository : ClientAssetRepository
    {
        private static readonly ConcurrentDictionary<string, IClientAsset> ASSET_MAP = new ConcurrentDictionary<string, IClientAsset>();
        public void Add(IClientAsset asset)
        {
            ASSET_MAP.AddOrUpdate(asset.Id, asset, (key, old) => asset);
        }
        public IEnumerable<IClientAsset> All()
        {
            return ASSET_MAP.Values;
        }
    }
}