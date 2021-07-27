namespace EventHorizon.Zone.System.DataStorage.Tests.Update
{
    using AutoFixture.Xunit2;
    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.System.DataStorage.Api;
    using EventHorizon.Zone.System.DataStorage.Events.Update;
    using EventHorizon.Zone.System.DataStorage.Save;
    using EventHorizon.Zone.System.DataStorage.Update;
    using FluentAssertions;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class UpdateDataStoreValueCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldSetKeyAndValueIntoDataStoreWhenCommandIsHandled(
            // Given
            [Frozen] Mock<DataStoreManagement> dataStoreManagementMock,
            UpdateDataStoreValueCommand command,
            UpdateDataStoreValueCommandHandler handler)
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
            [Frozen] Mock<DataStoreManagement> dataStoreManagementMock,
            UpdateDataStoreValueCommand command,
            UpdateDataStoreValueCommandHandler handler)
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
            [Frozen] Mock<IMediator> mediatorMock,
            UpdateDataStoreValueCommand command,
            UpdateDataStoreValueCommandHandler handler)
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
}
