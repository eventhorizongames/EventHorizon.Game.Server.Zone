
using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Tests.TestUtil;
using EventHorizon.TimerService;
using EventHorizon.Game.Server.Zone.ServerAction.Timer;
using EventHorizon.Game.Server.Zone.ServerAction;

namespace EventHorizon.Game.Server.Zone.Tests.ServerAction
{
    public class ServerActionExtensionsTests
    {
        [Fact]
        public void TestAddServerAction_ShouldConfigureServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            ServerActionExtensions.AddServerAction(serviceCollectionMock);

            // Then
            Assert.NotEmpty(serviceCollectionMock);
            Assert.Collection(serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(ITimerTask), service.ServiceType);
                    Assert.Equal(typeof(RunServerActionsTimerTask), service.ImplementationType);
                }
            );
        }
    }
}