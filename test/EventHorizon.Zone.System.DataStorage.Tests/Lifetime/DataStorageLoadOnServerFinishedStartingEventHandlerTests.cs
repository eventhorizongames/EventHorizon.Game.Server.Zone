namespace EventHorizon.Zone.System.DataStorage.Tests.Lifetime;

using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.System.DataStorage.Lifetime;
using EventHorizon.Zone.System.DataStorage.Load;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class DataStorageLoadOnServerFinishedStartingEventHandlerTests
{
    [Fact]
    public async Task ShouldSendLoadDataStoreWhenEventIsHandled()
    {
        // Given

        var mediatorMock = new Mock<IMediator>();

        // When
        var handler = new DataStorageLoadOnServerFinishedStartingEventHandler(
            mediatorMock.Object
        );
        await handler.Handle(
            new ServerFinishedStartingEvent(),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Send(
                new LoadDataStoreCommand(),
                CancellationToken.None
            )
        );

    }

}
