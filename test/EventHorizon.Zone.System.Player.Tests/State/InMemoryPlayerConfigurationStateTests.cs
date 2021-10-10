namespace EventHorizon.Zone.System.Player.Tests.State
{
    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Events.Json;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Player.State;
    using EventHorizon.Zone.System.Player.Tests.TestingModels;

    using FluentAssertions;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class InMemoryPlayerConfigurationStateTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldSetPlayerConfigurationWhenSetReturnsTrueForUpdated(
            // Given
            ObjectEntityConfigurationTestModel playerConfiguration,
            [Frozen] Mock<IMediator> mediatorMock,
            InMemoryPlayerConfigurationState state
        )
        {
            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<SerializeToJsonCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<SerializeToJsonResult>(
                    new SerializeToJsonResult(
                        "{}"
                    )
                )
            );

            // When
            var (Updated, OldConfig) = await state.Set(
                playerConfiguration,
                CancellationToken.None
            );
            var actual = state.PlayerConfiguration;

            // Then
            Updated.Should().BeTrue();
            OldConfig.Should().NotBeNull();

            actual.Should().BeEquivalentTo(
                playerConfiguration
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldReturnNotUpdatedAndOldPlayerConfigurationWhenSetIsRanTwiceWithSameConfiguration(
            // Given
            ObjectEntityConfigurationTestModel playerConfiguration,
            [Frozen] Mock<IMediator> mediatorMock,
            InMemoryPlayerConfigurationState state
        )
        {
            mediatorMock.Setup(
                mock => mock.Send(
                    new SerializeToJsonCommand(
                        playerConfiguration
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<SerializeToJsonResult>(
                    new SerializeToJsonResult(
                        "{}"
                    )
                )
            );

            // When
            await state.Set(
                playerConfiguration,
                CancellationToken.None
            );
            var (Updated, OldConfig) = await state.Set(
                playerConfiguration,
                CancellationToken.None
            );
            var actual = state.PlayerConfiguration;

            // Then
            Updated.Should().BeFalse();
            OldConfig.Should().BeEquivalentTo(
                playerConfiguration
            );

            actual.Should().BeEquivalentTo(
                playerConfiguration
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldReturnNotUpdatedWhenSetHasIssuesWithSerialization(
            // Given
            string errorCode,
            ObjectEntityConfigurationTestModel playerConfiguration,
            [Frozen] Mock<IMediator> mediatorMock,
            InMemoryPlayerConfigurationState state
        )
        {
            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<SerializeToJsonCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<SerializeToJsonResult>(
                    errorCode
                )
            );

            // When
            var (Updated, OldConfig) = await state.Set(
                playerConfiguration,
                CancellationToken.None
            );
            var actual = state.PlayerConfiguration;

            // Then
            Updated.Should().BeFalse();
            OldConfig.Should().NotBeSameAs(
                playerConfiguration
            );
        }
    }
}
