namespace EventHorizon.Identity.Tests.Client
{
    using System.Net.Http;

    using EventHorizon.Identity.Client;

    using FluentAssertions;

    using IdentityModel.Client;

    using Moq;

    using Xunit;

    public class CachingTokenClientFactoryTests
    {
        [Fact]
        public void TestShouldReturnNewTokenClientWhenCalledWithUniqueClientKey()
        {
            // Given
            var expectedUrl = "url";
            var expectedClientId = "client-id";
            var expectedClientSecret = "client-secret";

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            // When
            var tokenClientFactory = new CachingTokenClientFactory(
                httpClientFactoryMock.Object
            );
            var actual = tokenClientFactory.Create(
                expectedUrl,
                expectedClientId,
                expectedClientSecret
            );

            // Then
            Assert.NotNull(
                actual
            );
            actual.Should().BeOfType<TokenClient>();
        }
        [Fact]
        public void TestShouldReturnExistingClientWhenCalledMultipleTimesWithSameArguments()
        {
            // Given
            var url = "url";
            var clientId = "client-id";
            var clientSecret = "client-secret";

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            // When
            var tokenClientFactory = new CachingTokenClientFactory(
                httpClientFactoryMock.Object
            );
            var expected = tokenClientFactory.Create(
                url,
                clientId,
                clientSecret
            );
            var actual = tokenClientFactory.Create(
                url,
                clientId,
                clientSecret
            );

            // Then
            Assert.NotNull(
                expected
            );
            Assert.NotNull(
                actual
            );
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void TestShouldDisposeOfAnyExistingClientsWhenDisposed()
        {
            // Given
            var url = "url";
            var clientId = "client-id";
            var clientSecret = "client-secret";

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpClientMock = new Mock<HttpClient>();

            httpClientFactoryMock.Setup(
                mock => mock.CreateClient(
                    nameof(CachingTokenClientFactory)
                )
            ).Returns(
                httpClientMock.Object
            );

            // When
            var tokenClientFactory = new CachingTokenClientFactory(
                httpClientFactoryMock.Object
            );
            var expected = tokenClientFactory.Create(
                url,
                clientId,
                clientSecret
            );
            tokenClientFactory.Dispose();
            var actual = tokenClientFactory.Create(
                url,
                clientId,
                clientSecret
            );

            // Then
            expected.Should().NotBe(actual);
        }
    }
}
