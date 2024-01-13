namespace EventHorizon.Identity.Client;

using System;
using System.Collections.Concurrent;
using System.Net.Http;

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

    private readonly HttpClient _httpClient;

    public CachingTokenClientFactory(
        IHttpClientFactory httpClientFactory
    )
    {
        _httpClient = httpClientFactory.CreateClient(
            nameof(CachingTokenClientFactory)
        );
    }

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
            _httpClient,
            new TokenClientOptions
            {
                Address = url,
                ClientId = clientId,
                ClientSecret = clientSecret,
            }
        );

        _clientMap.AddOrUpdate(
            clientKey,
            client,
            (_, _) => client
        );
        return client;
    }

    private static string CreateClientKey(
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

            _httpClient.Dispose();

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
