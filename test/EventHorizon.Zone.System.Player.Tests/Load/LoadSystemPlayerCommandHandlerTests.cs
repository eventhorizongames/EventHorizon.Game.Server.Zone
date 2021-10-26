namespace EventHorizon.Zone.System.Player.Tests.Load
{
    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Player.Api;
    using EventHorizon.Zone.System.Player.Load;
    using EventHorizon.Zone.System.Player.Model;

    using FluentAssertions;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Moq;

    using Xunit;

    public class LoadSystemPlayerCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldReturnConfigurationNotFoundWhenFileLoaderReturnNull(
            // Given
            LoadSystemPlayerCommand request,
            [Frozen] Mock<IJsonFileLoader> fileLoader,
            LoadSystemPlayerCommandHandler handler
        )
        {
            var expected = "player_configuration_not_found";

            fileLoader.Setup(
                mock => mock.GetFile<PlayerObjectEntityConfigurationModel>(
                    It.IsAny<string>()
                )
            ).ReturnsAsync(
                (PlayerObjectEntityConfigurationModel)null
            );

            // When
            var actual = await handler.Handle(
                request,
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(
                    expected
                );
        }

        [Theory, AutoMoqData]
        public async Task ShouldReturnDataNotFoundWhenFileLoaderReturnNullForData(
            // Given
            LoadSystemPlayerCommand request,
            [Frozen] Mock<IJsonFileLoader> fileLoader,
            LoadSystemPlayerCommandHandler handler
        )
        {
            var expected = "player_data_not_found";

            fileLoader.Setup(
                mock => mock.GetFile<PlayerObjectEntityDataModel>(
                    It.IsAny<string>()
                )
            ).ReturnsAsync(
                (PlayerObjectEntityDataModel)null
            );

            // When
            var actual = await handler.Handle(
                request,
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(
                    expected
                );
        }

        [Theory, AutoMoqData]
        public async Task ShouldReturnWasUpdatedResultWhenPlayerConfigurationStateSetReturnsUpdatedSetCall(
            // Given
            LoadSystemPlayerCommand request,
            [Frozen] Mock<PlayerSettingsState> stateMock,
            LoadSystemPlayerCommandHandler handler
        )
        {
            var expected = new string[] { "player_configuration_changed" };

            stateMock.Setup(
                mock => mock.SetConfiguration(
                    It.IsAny<PlayerObjectEntityConfigurationModel>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                (true, null)
            );
            stateMock.Setup(
                mock => mock.SetData(
                    It.IsAny<PlayerObjectEntityDataModel>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                (false, null)
            );

            // When
            var actual = await handler.Handle(
                request,
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();
            actual.WasUpdated
                .Should().BeTrue();
            actual.ReasonCode
                .Should().BeEquivalentTo(expected);
        }

        [Theory, AutoMoqData]
        public async Task ShouldReturnWasUpdatedResultWhenPlayerDataStateSetReturnsUpdatedSetCall(
            // Given
            LoadSystemPlayerCommand request,
            [Frozen] Mock<PlayerSettingsState> stateMock,
            LoadSystemPlayerCommandHandler handler
        )
        {
            var expected = new string[] { "player_data_changed" };

            stateMock.Setup(
                mock => mock.SetConfiguration(
                    It.IsAny<PlayerObjectEntityConfigurationModel>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                (false, null)
            );
            stateMock.Setup(
                mock => mock.SetData(
                    It.IsAny<PlayerObjectEntityDataModel>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                (true, null)
            );

            // When
            var actual = await handler.Handle(
                request,
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();
            actual.WasUpdated
                .Should().BeTrue();
            actual.ReasonCode
                .Should().BeEquivalentTo(expected);
        }

        [Theory, AutoMoqData]
        public async Task ShouldReturnWasNotUpdatedResultWhenPlayerConfigurationStateSetReturnsNotUpdatedSetCall(
            // Given
            LoadSystemPlayerCommand request,
            [Frozen] Mock<PlayerSettingsState> stateMock,
            LoadSystemPlayerCommandHandler handler
        )
        {
            stateMock.Setup(
                mock => mock.SetConfiguration(
                    It.IsAny<PlayerObjectEntityConfigurationModel>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                (false, null)
            );
            stateMock.Setup(
                mock => mock.SetData(
                    It.IsAny<PlayerObjectEntityDataModel>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                (false, null)
            );

            // When
            var actual = await handler.Handle(
                request,
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();
            actual.WasUpdated
                .Should().BeFalse();
            actual.ReasonCode
                .Should().BeEmpty();
        }
    }
}
