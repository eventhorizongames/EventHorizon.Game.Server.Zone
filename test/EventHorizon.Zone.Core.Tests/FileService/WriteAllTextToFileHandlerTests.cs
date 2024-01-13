namespace EventHorizon.Zone.Core.Tests.FileService;

using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using Moq;
using Xunit;

public class WriteAllTextToFileHandlerTests
{
    [Fact]
    public async Task ShouldWriteAllTextWithRequestPropertiesWhenRequestIsHandled()
    {
        // Given
        var expectedFileFullName = "file-full-name";
        var expectedText = "file-full-text";

        var fileResolverMock = new Mock<FileResolver>();

        // When
        var handler = new WriteAllTextToFileHandler(fileResolverMock.Object);
        await handler.Handle(
            new WriteAllTextToFile(expectedFileFullName, expectedText),
            CancellationToken.None
        );

        // Then
        fileResolverMock.Verify(mock => mock.WriteAllText(expectedFileFullName, expectedText));
    }
}
