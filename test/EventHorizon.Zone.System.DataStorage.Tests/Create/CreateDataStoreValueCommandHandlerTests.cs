namespace EventHorizon.Zone.System.DataStorage.Tests.Create
{
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
        [Fact]
        public async Task ShouldSetKeyAndValueIntoDataStoreWhenCommandIsHandled()
        {
            // Given
            var key = "key";
            var type = "String";
            var value = "string";

            var mediatorMock = new Mock<IMediator>();
            var dataStoreManagementMock = new Mock<DataStoreManagement>();

            // When
            var handler = new CreateDataStoreValueCommandHandler(
                mediatorMock.Object,
                dataStoreManagementMock.Object
            );
            var actual = await handler.Handle(
                new CreateDataStoreValueCommand(
                    key,
                    type,
                    value
                ),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();
            dataStoreManagementMock.Verify(
                mock => mock.Set(
                    key,
                    value
                )
            );
        }

        [Fact]
        public async Task ShouldUpdateSchemaWithKeyAndTypeWhenCommandIsHandled()
        {
            // Given
            var key = "key";
            var type = "String";
            var value = "string";

            var mediatorMock = new Mock<IMediator>();
            var dataStoreManagementMock = new Mock<DataStoreManagement>();

            // When
            var handler = new CreateDataStoreValueCommandHandler(
                mediatorMock.Object,
                dataStoreManagementMock.Object
            );
            var actual = await handler.Handle(
                new CreateDataStoreValueCommand(
                    key,
                    type,
                    value
                ),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();
            dataStoreManagementMock.Verify(
                mock => mock.UpdateSchema(
                    key,
                    type
                )
            );
        }

        [Fact]
        public async Task ShouldPublishSaveDataStoreCommandWhenCommandIsHandled()
        {
            // Given
            var key = "key";
            var type = "String";
            var value = "string";

            var mediatorMock = new Mock<IMediator>();
            var dataStoreManagementMock = new Mock<DataStoreManagement>();

            // When
            var handler = new CreateDataStoreValueCommandHandler(
                mediatorMock.Object,
                dataStoreManagementMock.Object
            );
            var actual = await handler.Handle(
                new CreateDataStoreValueCommand(
                    key,
                    type,
                    value
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
