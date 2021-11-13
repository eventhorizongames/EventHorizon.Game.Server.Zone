namespace EventHorizon.Identity.Tests.AccessToken
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Identity.AccessToken;
    using EventHorizon.Identity.Client;
    using EventHorizon.Identity.Exceptions;
    using EventHorizon.Identity.Tests.TestUtil;
    using EventHorizon.Identity.Tests.TestUtils;

    using FluentAssertions;

    using IdentityModel.Client;

    using Moq;
    using Moq.Protected;

    using Newtonsoft.Json;

    using Xunit;

    public class RequestIdentityAccessTokenHandlerTests
    {
        [Fact]
        public async Task TestShouldGetAccessTokenWhenMakingRequestForAccessToken()
        {
            // Given
            var accessToken = "expected-access-token";
            var expected = accessToken;
            var authAuthority = "https://tokenAuthority";
            var tokenEndpointUrl = $"{authAuthority}/connect/token";
            var clientId = "client-id";
            var clientSecret = "client-secret";
            var apiName = "api-name";

            var configuration = new ConfigurationMock();
            var tokenClientFactoryMock = new Mock<ITokenClientFactory>();

            configuration["Auth:Authority"] = authAuthority;
            configuration["Auth:ClientId"] = clientId;
            configuration["Auth:ClientSecret"] = clientSecret;
            configuration["Auth:ApiName"] = apiName;

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(
                httpMessageHandlerMock.Object
            )
            {
                BaseAddress = new Uri(authAuthority)
            };
            var tokenClient = new TokenClient(
                httpClient,
                new TokenClientOptions()
            );
            tokenClientFactoryMock.Setup(
                mock => mock.Create(
                    tokenEndpointUrl,
                    clientId,
                    clientSecret
                )
            ).Returns(
                tokenClient
            );

            var tokenResponseString = string.Join(
                "",
                "{",
                "   \"access_token\": ", $"\"{accessToken}\",",
                "   \"id_token\": ", "\"\",",
                "   \"token_type\": ", "\"\",",
                "   \"refresh_token\": ", "\"\",",
                "   \"error_description\": ", "\"\"",
                "}"
            );

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(
                            tokenResponseString
                        ),
                    }
                ).Verifiable();

            // When
            var handler = new RequestIdentityAccessTokenHandler(
                configuration,
                tokenClientFactoryMock.Object
            );
            var actual = await handler.Handle(
                new RequestIdentityAccessTokenEvent(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task TestShouldThrowExceptionOnAnyTokenResponseError()
        {
            // Given
            var expectedMessage = "Error requesting token.";

            var authAuthority = "https://tokenAuthority";
            var tokenEndpointUrl = $"{authAuthority}/connect/token";
            var clientId = "client-id";
            var clientSecret = "client-secret";
            var apiName = "api-name";

            var configuration = new ConfigurationMock();
            var tokenClientFactoryMock = new Mock<ITokenClientFactory>();

            configuration["Auth:Authority"] = authAuthority;
            configuration["Auth:ClientId"] = clientId;
            configuration["Auth:ClientSecret"] = clientSecret;
            configuration["Auth:ApiName"] = apiName;

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(
                httpMessageHandlerMock.Object
            )
            {
                BaseAddress = new Uri(authAuthority)
            };
            var tokenClient = new TokenClient(
                httpClient,
                new TokenClientOptions()
            );
            tokenClientFactoryMock.Setup(
                mock => mock.Create(
                    tokenEndpointUrl,
                    clientId,
                    clientSecret
                )
            ).Returns(
                tokenClient
            );

            var tokenResponseString = string.Join(
                "",
                "{",
                "   \"access_token\": ", $"\"\",",
                "   \"id_token\": ", "\"\",",
                "   \"token_type\": ", "\"\",",
                "   \"refresh_token\": ", "\"\",",
                "   \"error\": ", "\"Error\"",
                "}"
            );

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        ReasonPhrase = "Error",
                        Content = new StringContent(
                            tokenResponseString
                        )
                    }
                ).Verifiable();

            // When
            var handler = new RequestIdentityAccessTokenHandler(
                configuration,
                tokenClientFactoryMock.Object
            );

            //act
            async Task handlerAction() => await handler.Handle(
                new RequestIdentityAccessTokenEvent(),
                CancellationToken.None
            );
            //assert
            var actual = await Assert.ThrowsAsync<IdentityServerRequestException>(
                handlerAction
            );

            // Then
            actual.Message.Should().Be(
                expectedMessage
            );
            actual.InnerException.Should().BeNull();
        }
    }
}
