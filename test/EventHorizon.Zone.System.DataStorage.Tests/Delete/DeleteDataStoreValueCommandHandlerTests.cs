namespace EventHorizon.Zone.System.DataStorage.Tests.Delete
{
    using EventHorizon.Zone.System.DataStorage.Api;
    using EventHorizon.Zone.System.DataStorage.Delete;
    using EventHorizon.Zone.System.DataStorage.Events.Delete;
    using EventHorizon.Zone.System.DataStorage.Save;
    using FluentAssertions;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class DeleteDataStoreValueCommandHandlerTests
    {   
        [Fact]
        public async Task ShouldDeleteKeyFromDataStoreWhenCommandIsHandled()
        {
            // Given
            var key = "key";

            var mediatorMock = new Mock<IMediator>();
            var dataStoreManagementMock = new Mock<DataStoreManagement>();

            // When
            var handler = new DeleteDataStoreValueCommandHandler(
                mediatorMock.Object,
                dataStoreManagementMock.Object
            );
            var actual = await handler.Handle(
                new DeleteDataStoreValueCommand(
                    key
                ),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();
            dataStoreManagementMock.Verify(
                mock => mock.Delete(
                    key
                )
            );
        }

        [Fact]
        public async Task ShouldDeleteKeyFromSchemaWhenCommandIsHandled()
        {
            // Given
            var key = "key";

            var mediatorMock = new Mock<IMediator>();
            var dataStoreManagementMock = new Mock<DataStoreManagement>();

            // When
            var handler = new DeleteDataStoreValueCommandHandler(
                mediatorMock.Object,
                dataStoreManagementMock.Object
            );
            var actual = await handler.Handle(
                new DeleteDataStoreValueCommand(
                    key
                ),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();
            dataStoreManagementMock.Verify(
                mock => mock.DeleteFromSchema(
                    key
                )
            );
        }

        [Fact]
        public async Task ShouldPublishSaveDataStoreCommandWhenCommandIsHandled()
        {
            // Given
            var key = "key";

            var mediatorMock = new Mock<IMediator>();
            var dataStoreManagementMock = new Mock<DataStoreManagement>();

            // When
            var handler = new DeleteDataStoreValueCommandHandler(
                mediatorMock.Object,
                dataStoreManagementMock.Object
            );
            var actual = await handler.Handle(
                new DeleteDataStoreValueCommand(
                    key
                ),
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
