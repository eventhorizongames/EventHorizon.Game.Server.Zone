namespace EventHorizon.Zone.System.DataStorage.Tests.Create;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.DataStorage.Api;
using EventHorizon.Zone.System.DataStorage.Create;
using EventHorizon.Zone.System.DataStorage.Events.Create;
using EventHorizon.Zone.System.DataStorage.Save;

using FluentAssertions;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class CreateDataStoreValueCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldSetKeyAndValueIntoDataStoreWhenCommandIsHandled(
        // Given
        CreateDataStoreValueCommand command,
        [Frozen] Mock<DataStoreManagement> dataStoreManagementMock,
        CreateDataStoreValueCommandHandler handler
    )
    {
        // When
        var actual = await handler.Handle(
            command,
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
        dataStoreManagementMock.Verify(
            mock => mock.Set(
                command.Key,
                command.Value
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldUpdateSchemaWithKeyAndTypeWhenCommandIsHandled(
        // Given
        CreateDataStoreValueCommand command,
        [Frozen] Mock<DataStoreManagement> dataStoreManagementMock,
        CreateDataStoreValueCommandHandler handler
    )
    {
        // When
        var actual = await handler.Handle(
            command,
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
        dataStoreManagementMock.Verify(
            mock => mock.UpdateSchema(
                command.Key,
                command.Type
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldPublishSaveDataStoreCommandWhenCommandIsHandled(
        // Given
        CreateDataStoreValueCommand command,
        [Frozen] Mock<IMediator> mediatorMock,
        CreateDataStoreValueCommandHandler handler)
    {
        // When
        var actual = await handler.Handle(
            command,
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
        mediatorMock.Verify(
            mock => mock.Send(
                new SaveDataStoreCommand(),
                CancellationToken.None
            )
        );
    }
}
