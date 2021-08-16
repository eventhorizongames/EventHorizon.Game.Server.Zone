namespace EventHorizon.TimerService.Tests.TimerService
{
    using EventHorizon.Test.Common.Utils;

    using Microsoft.Extensions.Hosting;

    using Xunit;

    public class TimerExtensionsTests
    {
        [Fact]
        public void TestAddTimer_ShouldConfigureServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            TimerExtensions.AddTimer(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(
                        typeof(IHostedService),
                        service.ServiceType
                    );
                    Assert.Equal(
                        typeof(TimerHostedService),
                        service.ImplementationType
                    );
                }
            );
        }
    }
}
