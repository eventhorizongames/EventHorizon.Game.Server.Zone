using System.Net.Http;

namespace EventHorizon.Game.Server.Zone.Core.Factory.Impl
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private static readonly HttpClient GLOBAL_CLIENT = new HttpClient();
        public HttpClient Client()
        {
            return GLOBAL_CLIENT;
        }
    }
}