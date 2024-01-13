namespace EventHorizon.Zone.Core.Tests.FileService;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.FileService;
using EventHorizon.Zone.Core.Model.FileService;

using FluentAssertions;

using Moq;

using Xunit;

public class ReadAllTextFromFileHandlerTests
{
    [Fact]
    public async Task ShouldGetFileTextWithRequestPropertiesWhenRequestIsHandled()
    {
        // Given
        var expected = "file-text";
        var fileFullName = "file-full-name";

        var fileResolverMock = new Mock<FileResolver>();

        fileResolverMock.Setup(
            mock => mock.GetFileText(
                fileFullName
            )
        ).Returns(
            expected
        );

        // When
        var handler = new ReadAllTextFromFileHandler(
            fileResolverMock.Object
        );
        var actual = await handler.Handle(
            new ReadAllTextFromFile(
                fileFullName
            ),
            CancellationToken.None
        );

        // Then
        actual.Should()
            .Be(
                expected
            );
        fileResolverMock.Verify(
            mock => mock.GetFileText(
                fileFullName
            )
        );
    }
}
