namespace EventHorizon.Identity.AccessToken
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Identity.Client;
    using EventHorizon.Identity.Exceptions;

    using IdentityModel.Client;

    using MediatR;

    using Microsoft.Extensions.Configuration;

    public class RequestIdentityAccessTokenHandler : IRequestHandler<RequestIdentityAccessTokenEvent, string>
    {
        readonly IConfiguration _configuration;
        readonly ITokenClientFactory _tokenClientFactory;

        public RequestIdentityAccessTokenHandler(
            IConfiguration configuration,
            ITokenClientFactory tokenClientFactory
        )
        {
            _configuration = configuration;
            _tokenClientFactory = tokenClientFactory;
        }

        public async Task<string> Handle(
            RequestIdentityAccessTokenEvent message,
            CancellationToken cancellationToken
        )
        {
            var tokenEndpoint = _configuration["Auth:Authority"];
            var clientId = _configuration["Auth:ClientId"];
            var clientSecret = _configuration["Auth:ClientSecret"];
            var apiScope = _configuration["Auth:ApiName"];
            // request token
            var tokenClient = _tokenClientFactory.Create(
                $"{tokenEndpoint}/connect/token",
                clientId,
                clientSecret
            );
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(
                apiScope
            );

            if (tokenResponse.IsError)
            {
                throw new IdentityServerRequestException(
                    "Error requesting token.",
                    tokenResponse.Exception
                );
            }

            return tokenResponse.AccessToken;
        }
    }
}
