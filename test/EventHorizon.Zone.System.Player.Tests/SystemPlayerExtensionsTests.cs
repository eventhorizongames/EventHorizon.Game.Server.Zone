namespace EventHorizon.Zone.System.Player.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.Player.Api;
    using EventHorizon.Zone.System.Player.Load;
    using EventHorizon.Zone.System.Player.State;

    using FluentAssertions;

    using global::System;
    using global::System.Threading;

    using MediatR;

    using Moq;

    using Xunit;

    public class SystemPlayerExtensionsTests
    {
        [Fact]
        public void ShouldAddExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var playerConfigurationStateMock = new Mock<PlayerSettingsState>();

            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(PlayerSettingsState)
                )
            ).Returns(
                playerConfigurationStateMock.Object
            );

            // When
            var actual = SystemPlayerExtensions.AddSystemPlayer(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                actual,
                service =>
                {
                    Assert.Equal(typeof(PlayerSettingsState), service.ServiceType);
                    Assert.Equal(typeof(InMemoryPlayerSettingsState), service.ImplementationType);
                },
                service =>
                {
                    service.ServiceType
                        .Should()
                        .Be(typeof(PlayerSettingsCache));
                    service.ImplementationFactory(serviceProviderMock.Object)
                        .Should()
                        .Be(playerConfigurationStateMock.Object);
                }
            );
        }

        [Fact]
        public void ShouldReturnApplicationBuilderForChainingCommands()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = new LoadSystemPlayerCommand();

            var mediatorMock = new Mock<IMediator>();

            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(IMediator))
            ).Returns(
                mediatorMock.Object
            );

            // When
            var actual = SystemPlayerExtensions.UseSystemPlayer(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }
}
