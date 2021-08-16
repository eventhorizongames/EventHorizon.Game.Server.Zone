namespace EventHorizon.Zone.System.Client.Scripts.Tests.Reload
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.System.Client.Scripts.Actions.Reload;
    using EventHorizon.Zone.System.Client.Scripts.Compile;
    using EventHorizon.Zone.System.Client.Scripts.Events.Reload;
    using EventHorizon.Zone.System.Client.Scripts.Load;
    using EventHorizon.Zone.System.Client.Scripts.Reload;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class ReloadClientScriptsSystemHandlerTests
    {
        [Fact]
        public async Task ShouldCallLoadCompliePublishEventToAllWhenHandled()
        {
            // Given
            var expectedAction = "CLIENT_SCRIPTS_SYSTEM_RELOADED_CLIENT_ACTION_EVENT";

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ReloadClientScriptsSystemHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new ReloadClientScriptsSystem(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new LoadClientScriptsSystemCommand(),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new CompileClientScriptCommand(),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.Is<ClientActionGenericToAllEvent>(
                        a => a.Action == expectedAction
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}
