using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Tests.TestUtil;
using EventHorizon.TimerService;
using Microsoft.Extensions.Hosting;

namespace EventHorizon.Game.Server.Zone.Tests.TimerService
{
    public class TimerExtensionsTests
    {
        [Fact]
        public void TestAddTimer_ShouldConfigureServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            TimerExtensions.AddTimer(serviceCollectionMock);

            // Then
            Assert.NotEmpty(serviceCollectionMock);
            Assert.Collection(serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(IHostedService), service.ServiceType);
                    Assert.Equal(typeof(TimerHostedService), service.ImplementationType);
                }
            );
        }
    }
}