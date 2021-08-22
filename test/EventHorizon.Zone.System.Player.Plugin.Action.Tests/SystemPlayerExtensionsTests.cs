namespace EventHorizon.Zone.System.Player.Tests
{
    using global::System.Threading;

    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Tests.TestUtils;
    using EventHorizon.Zone.System.Player.Plugin.Action.Events.Register;
    using EventHorizon.Zone.System.Player.Plugin.Action.State;

    using MediatR;

    using Moq;

    using Xunit;

    public class SystemPlayerPluginActionExtensionsTests
    {
        [Fact]
        public void TestAddServerSetup_ShouldAddExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemPlayerPluginActionExtensions.AddSystemPlayerPluginAction(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(PlayerActionRepository), service.ServiceType);
                    Assert.Equal(typeof(InMemoryPlayerActionRepository), service.ImplementationType);
                }
            );
        }

        [Fact]
        public void TestShouldReturnApplicationBuilderForChainingCommands()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = applicationBuilderMocks.ApplicationBuilderMock.Object;

            var mediatorMock = new Mock<IMediator>();

            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(IMediator))
            ).Returns(
                mediatorMock.Object
            );

            // When
            var actual = SystemPlayerPluginActionExtensions.UseSystemPlayerPluginAction(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
            mediatorMock.Verify(
                mock => mock.Publish(
                    new ReadyForPlayerActionRegistration(),
                    CancellationToken.None
                )
            );
        }
    }
}
