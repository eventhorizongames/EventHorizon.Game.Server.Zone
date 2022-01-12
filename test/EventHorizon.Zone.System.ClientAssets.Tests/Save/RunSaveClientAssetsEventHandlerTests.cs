namespace EventHorizon.Zone.System.ClientAssets.Tests.Save;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.ClientAssets.Save;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class RunSaveClientAssetsEventHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldSendSaveClientAssetsCommandWhenEventIsHandled(
        // Given
        [Frozen]
            Mock<IMediator> mediatorMock,
        RunSaveClientAssetsEventHandler handler
    )
    {
        var expected = new SaveClientAssetsCommand();

        // When
        await handler.Handle(
            new RunSaveClientAssetsEvent(),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            a => a.Send(expected, CancellationToken.None)
        );
    }
}
