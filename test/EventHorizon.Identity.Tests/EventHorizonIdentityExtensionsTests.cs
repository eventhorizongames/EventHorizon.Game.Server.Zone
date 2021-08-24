namespace EventHorizon.Identity.Tests
{
    using System;

    using EventHorizon.Identity.Client;
    using EventHorizon.Identity.Model;
    using EventHorizon.Tests.TestUtils;

    using Microsoft.Extensions.Options;

    using Xunit;

    public class EventHorizonIdentityExtensionsTests
    {
        [Fact]
        public void TestShouldReturnPassedInIServiceCollection()
        {
            // Given
            var serviceCollection = new ServiceCollectionMock();
            var expected = serviceCollection;
            static void configureAuthSettings(AuthSettings options) { }

            // When
            var actual = EventHorizonIdentityExtensions.AddEventHorizonIdentity(
                serviceCollection,
                configureAuthSettings
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
            Assert.Contains(
                actual,
                service =>
                    service.ServiceType == typeof(ITokenClientFactory)
                    &&
                    service.ImplementationType == typeof(CachingTokenClientFactory)
            );
            Assert.Contains(
                actual,
                service =>
                    service.ServiceType == typeof(IConfigureOptions<AuthSettings>)
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
