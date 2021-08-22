namespace EventHorizon.Zone.Core.ServerAction.Tests
{

    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Tests.TestUtils;
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.ServerAction.State;
    using EventHorizon.Zone.Core.ServerAction.Timer;

    using Xunit;

    public class CoreServerActionExtensionsTests
    {
        [Fact]
        public void TestAddServerAction_ShouldConfigureServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            CoreServerActionExtensions.AddCoreServerAction(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(IServerActionQueue), service.ServiceType);
                    Assert.Equal(typeof(ServerActionQueue), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(ITimerTask), service.ServiceType);
                    Assert.Equal(typeof(RunServerActionsTimerTask), service.ImplementationType);
                }
            );
        }
    }
}
