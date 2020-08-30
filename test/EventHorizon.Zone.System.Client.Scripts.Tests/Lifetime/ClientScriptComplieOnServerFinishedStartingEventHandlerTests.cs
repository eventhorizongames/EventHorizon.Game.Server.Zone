namespace EventHorizon.Zone.System.Client.Scripts.Tests.Lifetime
{
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.System.Client.Scripts.Compile;
    using EventHorizon.Zone.System.Client.Scripts.Lifetime;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class ClientScriptComplieOnServerFinishedStartingEventHandlerTests
    {
        [Fact]
        public async Task ShouldTriggerCompileClientScriptCommand()
        {
            // Given
            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ClientScriptComplieOnServerFinishedStartingEventHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new ServerFinishedStartingEvent(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new CompileClientScriptCommand(),
                    CancellationToken.None
                )
            );
        }
    }
}
