using System.Net.Http;

namespace EventHorizon.Game.Server.Zone.Core.Factory
{
    public interface IHttpClientFactory
    {
        HttpClient Client();
    }
}