namespace EventHorizon.Identity.Client
{
    using System;
    using System.Collections.Concurrent;

    using IdentityModel.Client;

    public interface ITokenClientFactory
    {
        TokenClient Create(
            string url,
            string clientId,
            string clientSecret
        );
    }

    public class CachingTokenClientFactory
        : ITokenClientFactory,
        IDisposable
    {
        private readonly ConcurrentDictionary<string, TokenClient> _clientMap = new();

        public TokenClient Create(
            string url,
            string clientId,
            string clientSecret
        )
        {
            var clientKey = CreateClientKey(
                url,
                clientId,
                clientSecret
            );
            if (_clientMap.TryGetValue(
                clientKey,
                out TokenClient? client
            ))
            {
                return client;
            }
            client = new TokenClient(
                url,
                clientId,
                clientSecret
            );
            _clientMap.AddOrUpdate(
                clientKey,
                client,
                (_, oldClient) =>
                {
                    oldClient?.Dispose();
                    return client;
                }
            );
            return client;
        }

        private string CreateClientKey(
            string url,
            string clientId,
            string clientSecret
        )
        {
            return $"{url}_{clientId}_{clientSecret}";
        }

        #region IDisposable Support
        private bool _disposedValue = false;

        protected virtual void Dispose(
            bool disposing
        )
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects).
                }

                foreach (var client in _clientMap)
                {
                    client.Value.Dispose();
                }

                _clientMap.Clear();

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(
                true
            );
        }
        #endregion
    }
}
