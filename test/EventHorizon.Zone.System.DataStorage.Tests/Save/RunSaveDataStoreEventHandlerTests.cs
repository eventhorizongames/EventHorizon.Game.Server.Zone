namespace EventHorizon.Zone.System.DataStorage.Tests.Save
{
    using EventHorizon.Zone.System.DataStorage.Save;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class RunSaveDataStoreEventHandlerTests
    {
        [Fact]
        public async Task ShouldSendSaveDataStoreCommandWhenHandled()
        {
            // Given
            var expected = new SaveDataStoreCommand();

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new RunSaveDataStoreEventHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new RunSaveDataStoreEvent(),
                CancellationToken.None
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
