namespace EventHorizon.Zone.System.Server.Scripts.Tests.Reload
{
    using EventHorizon.Zone.System.Server.Scripts.Complie;
    using EventHorizon.Zone.System.Server.Scripts.Events.Reload;
    using EventHorizon.Zone.System.Server.Scripts.Load;
    using EventHorizon.Zone.System.Server.Scripts.Reload;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;


    public class ReloadServerScriptsSystemCommandHandlerTests
    {
        [Fact]
        public async Task ShouldTriggerCommandsToReloadCompliedScriptsWhenCommandIsHandled()
        {
            // Given
            var expectedLoadCommand = new LoadServerScriptsCommand();
            var expectedCompileCommand = new CompileServerScriptsFromSubProcessCommand();

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ReloadServerScriptsSystemCommandHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new ReloadServerScriptsSystemCommand(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedLoadCommand,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedCompileCommand,
                    CancellationToken.None
                )
            );
        }
    }
}
