namespace EventHorizon.Identity.Tests.AccessToken
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Identity.AccessToken;
    using EventHorizon.Identity.Client;
    using EventHorizon.Identity.Exceptions;
    using EventHorizon.Identity.Tests.TestUtils;

    using IdentityModel.Client;

    using Moq;

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
            var authAuthority = "tokenAuthority";
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

            var tokenClientMock = new Mock<TokenClient>(
                tokenEndpointUrl
            );
            tokenClientFactoryMock.Setup(
                mock => mock.Create(
                    tokenEndpointUrl,
                    clientId,
                    clientSecret
                )
            ).Returns(
                tokenClientMock.Object
            );

            var tokenResponseMock = new TokenResponse(
                HttpStatusCode.OK,
                "OK",
                JsonConvert.SerializeObject(
                    new
                    {
                        access_token = accessToken
                    }
                )
            );

            tokenClientMock.Setup(
                mock => mock.RequestAsync(
                    It.IsAny<IDictionary<string, string>>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                tokenResponseMock
            );

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
            Assert.Equal(
                expected,
                actual
            );

            tokenClientMock.Verify(
                mock => mock.RequestAsync(
                    It.IsAny<IDictionary<string, string>>(),
                    CancellationToken.None
                )
            );
        }
        [Fact]
        public async Task TestShouldThrowExceptionOnAnyTokenResponseError()
        {
            // Given
            var expectedMessage = "Error requesting token.";
            var expectedException = new Exception("error");

            var authAuthority = "tokenAuthority";
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

            var tokenClientMock = new Mock<TokenClient>(
                tokenEndpointUrl
            );
            tokenClientFactoryMock.Setup(
                mock => mock.Create(
                    tokenEndpointUrl,
                    clientId,
                    clientSecret
                )
            ).Returns(
                tokenClientMock.Object
            );

            var tokenResponseMock = new TokenResponse(
                HttpStatusCode.InternalServerError,
                "Error",
                ""
            );
            tokenResponseMock.Exception = expectedException;

            tokenClientMock.Setup(
                mock => mock.RequestAsync(
                    It.IsAny<IDictionary<string, string>>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                tokenResponseMock
            );

            // When
            var handler = new RequestIdentityAccessTokenHandler(
                configuration,
                tokenClientFactoryMock.Object
            );

            //act
            Func<Task> handlerAction = async () => await handler.Handle(
                new RequestIdentityAccessTokenEvent(),
                CancellationToken.None
            );
            //assert
            var actual = await Assert.ThrowsAsync<IdentityServerRequestException>(
                handlerAction
            );

            // Then
            Assert.Equal(
                expectedMessage,
                actual.Message
            );
            Assert.Equal(
                expectedException,
                actual.InnerException
            );
        }
    }
}
