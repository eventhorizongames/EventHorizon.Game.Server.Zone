using System.Net.Http;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Core.ClientApi
{
    public class HttpClientPostEvent : IRequest<HttpResponseMessage>
    {
        public string AccessToken { get; internal set; }
        public object Content { get; internal set; }
        public string Uri { get; internal set; }
    }
}