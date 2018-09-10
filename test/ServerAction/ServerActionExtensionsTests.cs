
using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Tests.TestUtil;
using EventHorizon.TimerService;
using EventHorizon.Game.Server.Zone.ServerAction.Timer;
using EventHorizon.Game.Server.Zone.ServerAction;
using EventHorizon.Game.Server.Zone.ServerAction.State;
using EventHorizon.Game.Server.Zone.ServerAction.State.Impl;

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
                },
                service =>
                {
                    Assert.Equal(typeof(IServerActionQueue), service.ServiceType);
                    Assert.Equal(typeof(ServerActionQueue), service.ImplementationType);
                }
            );
        }
    }
}