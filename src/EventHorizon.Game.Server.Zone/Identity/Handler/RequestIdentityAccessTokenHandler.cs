using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Identity.Exceptions;
using IdentityModel.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EventHorizon.Identity.Handler
{
    public class RequestIdentityAccessTokenHandler : IRequestHandler<RequestIdentityAccessTokenEvent, string>
    {
        private readonly IConfiguration _configuration;

        public RequestIdentityAccessTokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> Handle(RequestIdentityAccessTokenEvent message, CancellationToken cancellationToken)
        {
            var tokenEndpoint = _configuration["Auth:Authority"];
            var clientId = _configuration["Auth:ClientId"];
            var clientSecret = _configuration["Auth:ClientSecret"];
            var apiScope = _configuration["Auth:ApiName"];
            // request token
            var tokenClient = new TokenClient($"{tokenEndpoint}/connect/token", clientId, clientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(apiScope);

            if (tokenResponse.IsError)
            {
                throw new IdentityServerRequestException("Error requesting token.");
            }

            return tokenResponse.AccessToken;
        }
    }
}