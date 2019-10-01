using EventHorizon.Identity.Client;
using Xunit;

namespace EventHorizon.Identity.Tests.Client
{
    public class CachingTokenClientFactoryTests
    {
        [Fact]
        public void TestShouldReturnNewTokenClientWhenCalledWithUniqueClientKey()
        {
            // Given
            var expectedUrl = "url";
            var expectedClientId = "client-id";
            var expectedClientSecret = "client-secret";

            // When
            var tokenClientFactory = new CachingTokenClientFactory();
            var actual = tokenClientFactory.Create(
                expectedUrl,
                expectedClientId,
                expectedClientSecret
            );

            // Then
            Assert.NotNull(
                actual
            );
            Assert.Equal(
                expectedUrl,
                actual.Address
            );
            Assert.Equal(
                expectedClientId,
                actual.ClientId
            );
            Assert.Equal(
                expectedClientSecret,
                actual.ClientSecret
            );
        }
        [Fact]
        public void TestShouldReturnExistingClientWhenCalledMultipleTimesWithSameArguments()
        {
            // Given
            var url = "url";
            var clientId = "client-id";
            var clientSecret = "client-secret";

            // When
            var tokenClientFactory = new CachingTokenClientFactory();
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

            // When
            var tokenClientFactory = new CachingTokenClientFactory();
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
            Assert.NotNull(
                expected
            );
            Assert.NotNull(
                actual
            );
            Assert.NotEqual(
                expected,
                actual
            );
        }
    }
}