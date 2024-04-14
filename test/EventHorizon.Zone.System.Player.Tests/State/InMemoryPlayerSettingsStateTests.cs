namespace EventHorizon.Zone.System.Player.Tests.State;

using AutoFixture.Xunit2;
using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.Json;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Player.Model.Settings;
using EventHorizon.Zone.System.Player.State;
using EventHorizon.Zone.System.Player.Tests.TestingModels;
using FluentAssertions;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;
using Moq;
using Xunit;

public class InMemoryPlayerSettingsStateTests
{
    [Theory, AutoMoqData]
    public async Task ShouldSetPlayerConfigurationWhenSetReturnsTrueForUpdated(
        // Given
        ObjectEntityConfigurationTestModel playerConfiguration,
        [Frozen] Mock<IMediator> mediatorMock,
        InMemoryPlayerSettingsState state
    )
    {
        mediatorMock
            .Setup(mock => mock.Send(It.IsAny<SerializeToJsonCommand>(), CancellationToken.None))
            .ReturnsAsync(
                new CommandResult<SerializeToJsonResult>(new SerializeToJsonResult("{}"))
            );

        // When
        var (Updated, OldConfig) = await state.SetConfiguration(
            playerConfiguration,
            CancellationToken.None
        );
        var actual = state.PlayerConfiguration;

        // Then
        Updated.Should().BeTrue();
        OldConfig.Should().NotBeNull();

        actual.Should().BeEquivalentTo(playerConfiguration);
    }

    [Theory, AutoMoqData]
    public async Task ShouldReturnNotUpdatedAndOldPlayerConfigurationWhenSetIsRanTwiceWithSameConfiguration(
        // Given
        ObjectEntityConfigurationTestModel playerConfiguration,
        [Frozen] Mock<IMediator> mediatorMock,
        InMemoryPlayerSettingsState state
    )
    {
        mediatorMock
            .Setup(mock =>
                mock.Send(new SerializeToJsonCommand(playerConfiguration), CancellationToken.None)
            )
            .ReturnsAsync(
                new CommandResult<SerializeToJsonResult>(new SerializeToJsonResult("{}"))
            );

        // When
        await state.SetConfiguration(playerConfiguration, CancellationToken.None);
        var (Updated, OldConfig) = await state.SetConfiguration(
            playerConfiguration,
            CancellationToken.None
        );
        var actual = state.PlayerConfiguration;

        // Then
        Updated.Should().BeFalse();
        OldConfig.Should().BeEquivalentTo(playerConfiguration);

        actual.Should().BeEquivalentTo(playerConfiguration);
    }

    [Theory, AutoMoqData]
    public async Task ShouldReturnNotUpdatedConfigurationWhenSetHasIssuesWithSerialization(
        // Given
        string errorCode,
        ObjectEntityConfigurationTestModel playerConfiguration,
        [Frozen] Mock<IMediator> mediatorMock,
        InMemoryPlayerSettingsState state
    )
    {
        mediatorMock
            .Setup(mock => mock.Send(It.IsAny<SerializeToJsonCommand>(), CancellationToken.None))
            .ReturnsAsync(new CommandResult<SerializeToJsonResult>(errorCode));

        // When
        var (Updated, OldConfig) = await state.SetConfiguration(
            playerConfiguration,
            CancellationToken.None
        );
        var actual = state.PlayerConfiguration;

        // Then
        Updated.Should().BeFalse();
        OldConfig.Should().NotBeSameAs(playerConfiguration);
    }

    [Theory, AutoMoqData]
    public async Task ShouldSetPlayerDataWhenSetReturnsTrueForUpdated(
        // Given
        ObjectEntityDataTestModel playerData,
        [Frozen] Mock<IMediator> mediatorMock,
        InMemoryPlayerSettingsState state
    )
    {
        mediatorMock
            .Setup(mock => mock.Send(It.IsAny<SerializeToJsonCommand>(), CancellationToken.None))
            .ReturnsAsync(
                new CommandResult<SerializeToJsonResult>(new SerializeToJsonResult("{}"))
            );

        // When
        var (Updated, OldData) = await state.SetData(playerData, CancellationToken.None);
        var actual = state.PlayerData;

        // Then
        Updated.Should().BeTrue();
        OldData.Should().NotBeNull();

        actual.Should().BeEquivalentTo(playerData);
    }

    [Theory, AutoMoqData]
    public async Task ShouldReturnNotUpdatedAndOldPlayerDataWhenSetIsRanTwiceWithSameData(
        // Given
        ObjectEntityDataTestModel playerData,
        [Frozen] Mock<IMediator> mediatorMock,
        InMemoryPlayerSettingsState state
    )
    {
        mediatorMock
            .Setup(mock =>
                mock.Send(new SerializeToJsonCommand(playerData), CancellationToken.None)
            )
            .ReturnsAsync(
                new CommandResult<SerializeToJsonResult>(new SerializeToJsonResult("{}"))
            );

        // When
        await state.SetData(playerData, CancellationToken.None);
        var (Updated, OldData) = await state.SetData(playerData, CancellationToken.None);
        var actual = state.PlayerData;

        // Then
        Updated.Should().BeFalse();
        OldData.Should().BeEquivalentTo(playerData);

        actual.Should().BeEquivalentTo(playerData);
    }

    [Theory, AutoMoqData]
    public async Task ShouldReturnNotUpdatedDataWhenSetHasIssuesWithSerialization(
        // Given
        string errorCode,
        PlayerObjectEntityDataModel playerData,
        [Frozen] Mock<IMediator> mediatorMock,
        InMemoryPlayerSettingsState state
    )
    {
        mediatorMock
            .Setup(mock => mock.Send(It.IsAny<SerializeToJsonCommand>(), CancellationToken.None))
            .ReturnsAsync(new CommandResult<SerializeToJsonResult>(errorCode));

        // When
        var (Updated, OldConfig) = await state.SetData(playerData, CancellationToken.None);
        var actual = state.PlayerData;

        // Then
        Updated.Should().BeFalse();
        OldConfig.Should().NotBeSameAs(playerData);
    }
}
