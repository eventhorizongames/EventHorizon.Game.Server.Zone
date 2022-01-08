namespace EventHorizon.Zone.System.Gui.Tests.Register;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.Gui.Api;
using EventHorizon.Zone.System.Gui.Model;
using EventHorizon.Zone.System.Gui.Register;

using global::System.Threading;
using global::System.Threading.Tasks;

using Moq;

using Xunit;

public class RegisterGuiLayoutHandlerTests
{
    [Theory, AutoMoqData(disableRecursionCheck: true)]
    public async Task AddsTheLayoutToTheStateWhenCommandIsHandled(
        // Given
        GuiLayout layout,
        [Frozen] Mock<GuiState> stateMock,
        RegisterGuiLayoutCommandHandler handler
    )
    {
        // When
        await handler.Handle(
            new RegisterGuiLayoutCommand(
                layout
            ),
            CancellationToken.None
        );

        // Then
        stateMock.Verify(
            mock => mock.AddLayout(
                layout.Id,
                layout
            )
        );
    }
}
