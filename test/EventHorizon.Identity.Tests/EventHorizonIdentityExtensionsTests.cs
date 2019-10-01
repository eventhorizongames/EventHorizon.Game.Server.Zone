using EventHorizon.Game.Server.Zone;
using Xunit;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using EventHorizon.Tests.TestUtils;
using EventHorizon.Identity.Client;

namespace EventHorizon.Identity.Tests
{
    public class EventHorizonIdentityExtensionsTests
    {
        [Fact]
        public void TestShouldReturnPassedInIServiceCollection()
        {
            // Given
            var serviceCollection = new ServiceCollectionMock();
            var expected = serviceCollection;

            var configurationMock = new Mock<IConfiguration>();

            // When
            var actual = EventHorizonIdentityExtensions.AddEventHorizonIdentity(
                serviceCollection,
                configurationMock.Object
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
            Assert.Collection(
                actual,
                service =>
                {
                    Assert.Equal(typeof(ITokenClientFactory), service.ServiceType);
                    Assert.Equal(typeof(CachingTokenClientFactory), service.ImplementationType);
                }
            );
        }
        [Fact]
        public void TestShouldRunWithoutFailing()
        {
            // Given
            var mocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = mocks.ApplicationBuilderMock.Object;

            // When
            var actual = EventHorizonIdentityExtensions.UseEventHorizonIdentity(
                mocks.ApplicationBuilderMock.Object
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}