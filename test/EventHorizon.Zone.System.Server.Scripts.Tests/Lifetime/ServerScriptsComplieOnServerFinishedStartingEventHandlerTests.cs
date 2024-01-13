namespace EventHorizon.Zone.System.Server.Scripts.Tests.Lifetime;

using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.System.Client.Server.Lifetime;
using EventHorizon.Zone.System.Server.Scripts.Complie;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;


public class ServerScriptsComplieOnServerFinishedStartingEventHandlerTests
{
    [Fact]
    public async Task ShouldSendCompileScriptsCommandWhenEventIsHandled()
    {
        // Given
        var expected = new CompileServerScriptsFromSubProcessCommand();

        var mediatorMock = new Mock<IMediator>();

        // When
        var handler = new ServerScriptsComplieOnServerFinishedStartingEventHandler(
            mediatorMock.Object
        );
        await handler.Handle(
            new ServerFinishedStartingEvent(),
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
