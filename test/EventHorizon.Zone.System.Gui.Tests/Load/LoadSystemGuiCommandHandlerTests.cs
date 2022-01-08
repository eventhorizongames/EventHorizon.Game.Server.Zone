namespace EventHorizon.Zone.System.Gui.Tests.Load;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Gui.Load;
using EventHorizon.Zone.System.Gui.Model;
using EventHorizon.Zone.System.Gui.Register;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class LoadSystemGuiCommandHandlerTests
{
    [Theory, AutoMoqData(disableRecursionCheck: true)]
    public async Task RegisterGuiLayoutWhenSuccessfulyLoadedFromClientGuiPath(
        // Given
        StandardFileInfo fileInfo,
        GuiLayout guiLayout,
        [Frozen] Mock<IMediator> senderMock,
        [Frozen] Mock<IJsonFileLoader> jsonFileLoaderMock,
        LoadSystemGuiCommandHandler handler
    )
    {
        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<GetListOfFilesFromDirectory>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new List<StandardFileInfo>
            {
                fileInfo,
            }
        );

        jsonFileLoaderMock.Setup(
            mock => mock.GetFile<GuiLayout>(
                It.IsAny<string>()
            )
        ).ReturnsAsync(
            guiLayout
        );

        // When
        await handler.Handle(
            new LoadSystemGuiCommand(),
            CancellationToken.None
        );

        // Then
        senderMock.Verify(
            mock => mock.Send(
                new RegisterGuiLayoutCommand(
                    guiLayout
                ),
                CancellationToken.None
            )
        );
    }

    [Theory, AutoMoqData(disableRecursionCheck: true)]
    public async Task DoesNotRegisterAnyGuidLayoutsWhenFileLoaderReturnsNull(
        // Given
        StandardFileInfo fileInfo,
        [Frozen] Mock<IMediator> senderMock,
        [Frozen] Mock<IJsonFileLoader> jsonFileLoaderMock,
        LoadSystemGuiCommandHandler handler
    )
    {
        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<GetListOfFilesFromDirectory>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new List<StandardFileInfo>
            {
                fileInfo,
            }
        );

        jsonFileLoaderMock.Setup(
            mock => mock.GetFile<GuiLayout>(
                It.IsAny<string>()
            )
        ).ReturnsAsync(
            default(GuiLayout)
        );

        // When
        await handler.Handle(
            new LoadSystemGuiCommand(),
            CancellationToken.None
        );

        // Then
        senderMock.Verify(
            mock => mock.Send(
                It.IsAny<RegisterGuiLayoutCommand>(),
                CancellationToken.None
            ),
            Times.Never()
        );
    }
}
