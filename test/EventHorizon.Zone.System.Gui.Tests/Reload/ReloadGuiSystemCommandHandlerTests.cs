namespace EventHorizon.Zone.System.Gui.Tests.Reload;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.Gui.Api;
using EventHorizon.Zone.System.Gui.Load;
using EventHorizon.Zone.System.Gui.Reload;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;


public class ReloadGuiSystemCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ClearAndTriggerLoadSystemGuiCommandWhenRequestIsHandled(
        // Given
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<GuiState> stateMock,
        ReloadGuiSystemCommandHandler handler
    )
    {
        // When
        await handler.Handle(
            new ReloadGuiSystemCommand(),
            CancellationToken.None
        );

        // Then
        senderMock.Verify(
            mock => mock.Send(
                new LoadSystemGuiCommand(),
                CancellationToken.None
            )
        );

        stateMock.Verify(
            mock => mock.Clear()
        );
    }
}
