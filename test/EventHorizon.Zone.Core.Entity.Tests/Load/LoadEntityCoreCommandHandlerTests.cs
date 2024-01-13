namespace EventHorizon.Zone.Core.Entity.Tests.Load;

using System.Threading;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Entity.Api;
using EventHorizon.Zone.Core.Entity.Load;
using EventHorizon.Zone.Core.Entity.Model;
using EventHorizon.Zone.Core.Model.Json;

using FluentAssertions;

using Moq;

using Xunit;

public class LoadEntityCoreCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldReturnConfigurationNotFoundWhenFileLoaderReturnNull(
        // Given
        LoadEntityCoreCommand request,
        [Frozen] Mock<IJsonFileLoader> fileLoader,
        LoadEntityCoreCommandHandler handler
    )
    {
        var expected = "entity_configuration_not_found";

        fileLoader.Setup(
            mock => mock.GetFile<ObjectEntityConfigurationModel>(
                It.IsAny<string>()
            )
        ).ReturnsAsync(
            (ObjectEntityConfigurationModel)null
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
        LoadEntityCoreCommand request,
        [Frozen] Mock<IJsonFileLoader> fileLoader,
        LoadEntityCoreCommandHandler handler
    )
    {
        var expected = "entity_data_not_found";

        fileLoader.Setup(
            mock => mock.GetFile<ObjectEntityDataModel>(
                It.IsAny<string>()
            )
        ).ReturnsAsync(
            (ObjectEntityDataModel)null
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
    public async Task ShouldReturnWasUpdatedResultWhenEntityConfigurationStateSetReturnsUpdatedSetCall(
        // Given
        LoadEntityCoreCommand request,
        [Frozen] Mock<EntitySettingsState> stateMock,
        LoadEntityCoreCommandHandler handler
    )
    {
        var expected = new string[] { "entity_configuration_changed" };

        stateMock.Setup(
            mock => mock.SetConfiguration(
                It.IsAny<ObjectEntityConfigurationModel>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            (true, null)
        );
        stateMock.Setup(
            mock => mock.SetData(
                It.IsAny<ObjectEntityDataModel>(),
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
    public async Task ShouldReturnWasUpdatedResultWhenEntityDataStateSetReturnsUpdatedSetCall(
        // Given
        LoadEntityCoreCommand request,
        [Frozen] Mock<EntitySettingsState> stateMock,
        LoadEntityCoreCommandHandler handler
    )
    {
        var expected = new string[] { "entity_data_changed" };

        stateMock.Setup(
            mock => mock.SetConfiguration(
                It.IsAny<ObjectEntityConfigurationModel>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            (false, null)
        );
        stateMock.Setup(
            mock => mock.SetData(
                It.IsAny<ObjectEntityDataModel>(),
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
    public async Task ShouldReturnWasUpdatedResultWhenEntityConfigurationStateAndDataSetSetReturnsUpdatedSetCall(
        // Given
        LoadEntityCoreCommand request,
        [Frozen] Mock<EntitySettingsState> stateMock,
        LoadEntityCoreCommandHandler handler
    )
    {
        var expected = new string[] { "entity_configuration_changed", "entity_data_changed" };

        stateMock.Setup(
            mock => mock.SetConfiguration(
                It.IsAny<ObjectEntityConfigurationModel>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            (true, null)
        );
        stateMock.Setup(
            mock => mock.SetData(
                It.IsAny<ObjectEntityDataModel>(),
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
    public async Task ShouldReturnWasNotUpdatedResultWhenEntityConfigurationStateSetReturnsNotUpdatedSetCall(
        // Given
        LoadEntityCoreCommand request,
        [Frozen] Mock<EntitySettingsState> stateMock,
        LoadEntityCoreCommandHandler handler
    )
    {
        stateMock.Setup(
            mock => mock.SetConfiguration(
                It.IsAny<ObjectEntityConfigurationModel>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            (false, null)
        );
        stateMock.Setup(
            mock => mock.SetData(
                It.IsAny<ObjectEntityDataModel>(),
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
