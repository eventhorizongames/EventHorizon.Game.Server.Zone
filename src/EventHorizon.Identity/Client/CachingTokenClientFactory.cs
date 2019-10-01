using System;
using System.Collections.Concurrent;
using IdentityModel.Client;

namespace EventHorizon.Identity.Client
{
    public interface ITokenClientFactory
    {
        TokenClient Create(
            string url,
            string clientId,
            string clientSecret
        );
    }
    public class CachingTokenClientFactory : ITokenClientFactory, IDisposable
    {
        private readonly ConcurrentDictionary<string, TokenClient> CLIENT_MAP = new ConcurrentDictionary<string, TokenClient>();

        public TokenClient Create(
            string url,
            string clientId,
            string clientSecret
        )
        {
            var client = default(TokenClient);
            var clientKey = CreateClientKey(
                url,
                clientId,
                clientSecret
            );
            if (CLIENT_MAP.TryGetValue(
                clientKey,
                out client
            ))
            {
                return client;
            }
            client = new TokenClient(
                url,
                clientId,
                clientSecret
            );
            CLIENT_MAP.AddOrUpdate(
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
        private bool disposedValue = false;

        protected virtual void Dispose(
            bool disposing
        )
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects).
                }

                foreach (var client in CLIENT_MAP)
                {
                    client.Value.Dispose();
                }

                CLIENT_MAP.Clear();

                disposedValue = true;
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