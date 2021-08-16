namespace EventHorizon.Zone.System.ClientAssets.State
{
    using EventHorizon.Zone.System.ClientAssets.Model;
    using EventHorizon.Zone.System.ClientAssets.State.Api;

    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;

    public class ClientAssetInMemoryRepository
        : ClientAssetRepository
    {
        private readonly ConcurrentDictionary<string, ClientAsset> _map = new();

        public void Add(
            ClientAsset asset
        )
        {
            _map.AddOrUpdate(
                asset.Id,
                asset,
                (_, __) => asset
            );
        }

        public IEnumerable<ClientAsset> All()
            => _map.Values;

        public void Delete(
            string id
        )
        {
            _map.Remove(
                id,
                out _
            );
        }

        public Option<ClientAsset> Get(
            string id
        )
        {
            if (_map.TryGetValue(
                id,
                out var value
            ))
            {
                return value;
            }

            return null;
        }

        public void Set(
            ClientAsset clientAsset
        ) => Add(
            clientAsset
        );
    }
}
