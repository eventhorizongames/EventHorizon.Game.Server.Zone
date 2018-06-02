using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Exceptions;
using IdentityModel.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Zone.Core.ClientApi.Handler
{
    public class HttpClientPostHandler : IRequestHandler<HttpClientPostEvent, HttpResponseMessage>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClientPostHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<HttpResponseMessage> Handle(HttpClientPostEvent message, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            // call api
            client.SetBearerToken(message.AccessToken);
            var jsonString = JsonConvert.SerializeObject(message.Content);

            return await client.PostAsync(message.Uri, new StringContent(jsonString, Encoding.UTF8, "application/json"));
        }
    }
}