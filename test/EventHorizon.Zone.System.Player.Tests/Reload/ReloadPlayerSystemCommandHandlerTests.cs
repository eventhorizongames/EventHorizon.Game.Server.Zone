namespace EventHorizon.Zone.System.Player.Tests.Reload
{
    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Player.Load;
    using EventHorizon.Zone.System.Player.Model.Client;
    using EventHorizon.Zone.System.Player.Reload;

    using FluentAssertions;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class ReloadPlayerSystemCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldTriggerLoadSystemPlayerCommandWhenReloadIsHandled(
            // Given
            bool wasUpdated,
            string[] reasonCode,
            [Frozen] Mock<IMediator> mediatorMock,
            ReloadPlayerSystemCommandHandler handler
        )
        {
            var command = new ReloadPlayerSystemCommand();

            mediatorMock.Setup(
                mock => mock.Send(
                    new LoadPlayerSystemCommand(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new LoadPlayerSystemResult(
                    wasUpdated,
                    reasonCode
                )
            );

            // When
            var actual = await handler.Handle(
                command,
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();

            mediatorMock.Verify(
                mock => mock.Send(
                    new LoadPlayerSystemCommand(),
                    CancellationToken.None
                )
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldPublishPlayerConfigurationWhenLoadSystemPlayerWasUpdated(
            // Given
            string[] reasonCode,
            [Frozen] Mock<ObjectEntityConfiguration> playerConfigurationMock,
            [Frozen] Mock<IMediator> mediatorMock,
            ReloadPlayerSystemCommandHandler handler
        )
        {
            var actual = default(ClientActionGenericToAllEvent);
            var wasUpdated = true;
            var command = new ReloadPlayerSystemCommand();
            var expectedData = new PlayerSystemReloadedEventData(
                playerConfigurationMock.Object
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new LoadPlayerSystemCommand(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new LoadPlayerSystemResult(
                    wasUpdated,
                    reasonCode
                )
            );

            mediatorMock.Setup(
                mock => mock.Publish(
                    It.IsAny<ClientActionGenericToAllEvent>(),
                    CancellationToken.None
                )
            ).Callback<ClientActionGenericToAllEvent, CancellationToken>(
                (request, _) =>
                {
                    actual = request;
                }
            );

            // When
            await handler.Handle(
                command,
                CancellationToken.None
            );

            // Then
            actual.Should().NotBeNull();
            actual.Data.Should().Be(
                expectedData
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldNotPublishPlayerReloadedClientActionWhenLoadSystemPlayerReturnsError(
            // Given
            string errorCode,
            [Frozen] Mock<IMediator> mediatorMock,
            ReloadPlayerSystemCommandHandler handler
        )
        {
            var command = new ReloadPlayerSystemCommand();

            mediatorMock.Setup(
                mock => mock.Send(
                    new LoadPlayerSystemCommand(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new LoadPlayerSystemResult(
                    errorCode
                )
            );

            // When
            await handler.Handle(
                command,
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<ClientActionGenericToAllEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
    }
}
