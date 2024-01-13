namespace EventHorizon.Zone.Core.Tests.DirectoryService;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.DirectoryService;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;

using FluentAssertions;

using Moq;

using Xunit;

public class GetListOfFilesFromDirectoryHandlerTests
{
    [Fact]
    public async Task ShouldReturnStandardFileInfoListForDirectorFullNameInRequest()
    {
        // Given
        var directoryFullName = "directory-full-name";
        var fileName = "file-name";
        var fileDirectoryName = "file-directory-name";
        var fileFullName = "file-full-name";
        var fileExtension = "file-ext";
        var expected = new[]{
            new StandardFileInfo(
                fileName,
                fileDirectoryName,
                fileFullName,
                fileExtension
            )
        }.ToList();

        var directoryResolverMock = new Mock<DirectoryResolver>();

        directoryResolverMock.Setup(
            mock => mock.GetFiles(
                directoryFullName
            )
        ).Returns(
            expected
        );

        // When
        var handler = new GetListOfFilesFromDirectoryHandler(
            directoryResolverMock.Object
        );
        var actual = await handler.Handle(
            new GetListOfFilesFromDirectory(
                directoryFullName
            ),
            CancellationToken.None
        );

        // Then
        actual.Should().BeEquivalentTo(
            expected
        );
        directoryResolverMock.Verify(
            mock => mock.GetFiles(
                directoryFullName
            )
        );
    }
}
