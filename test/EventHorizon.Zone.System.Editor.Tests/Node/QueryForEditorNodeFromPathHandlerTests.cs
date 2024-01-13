namespace EventHorizon.Zone.System.Editor.Tests.Node;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.System.Editor.Events.Node;
using EventHorizon.Zone.System.Editor.Node;

using FluentAssertions;

using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class QueryForEditorNodeFromPathHandlerTests
{
    [Fact]
    public async Task ShouldFillEditorNodeFilledWithFoldersAndFilesWhenRequestIsHandled()
    {
        // Given
        var nodePath = new List<string> { "node-path" };
        var rootDirectoryFullName = "root-directory-full-name";
        var directoryToLoadName = "directory-to-load-name";
        var directoryToLoadFullName = "directory-to-load-full-name";
        var nodeType = "node-type";

        var subDirectoryName = "sub-directory-name";
        var subDirectoryFullName = "sub-directory-full-name";
        var subDirectoryFileName = "sub-directory-file-name";
        var subDirectoryFileFullName = "sub-directory-file-full-name";
        var subDirectoryFileExtension = "sub-directory-file-extension";

        var request = new QueryForEditorNodeFromPath(
            nodePath,
            rootDirectoryFullName,
            directoryToLoadFullName,
            nodeType
        );

        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(
            mock => mock.Send(
                new GetDirectoryInfo(
                    directoryToLoadFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardDirectoryInfo(
                directoryToLoadName,
                directoryToLoadFullName,
                rootDirectoryFullName
            )
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new DoesDirectoryExist(
                    directoryToLoadFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            true
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new GetListOfDirectoriesFromDirectory(
                    directoryToLoadFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new List<StandardDirectoryInfo>
            {
                new StandardDirectoryInfo(
                    subDirectoryName,
                    subDirectoryFullName,
                    directoryToLoadFullName
                )
            }
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new GetListOfFilesFromDirectory(
                    subDirectoryFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new List<StandardFileInfo>
            {
                new StandardFileInfo(
                    subDirectoryFileName,
                    subDirectoryFullName,
                    subDirectoryFileFullName,
                    subDirectoryFileExtension
                )
            }
        );

        // When
        var handler = new QueryForEditorNodeFromPathHandler(
            mediatorMock.Object
        );
        var actual = await handler.Handle(
            request,
            CancellationToken.None
        );

        // Then
        // Check the Children on root is expected
        actual.Children
            .Should().NotBeEmpty()
            .And
            .HaveCount(1);
        // Check the Type on Root is expected
        actual.Type
            .Should().Be("FOLDER");
        // Check the First Child on Root is expected
        actual.Children
            .First()
            .Should().NotBeNull();
        actual.Children
            .First()
            .IsFolder
            .Should().BeTrue();
        // Check the First Child Children
        actual.Children
            .First()
            .Children
            .Should().NotBeEmpty()
            .And
            .HaveCount(1);
        actual.Children
            .First()
            .Children
            .First()
            .IsFolder
            .Should().BeFalse();
    }

    [Theory]
    [InlineData(".js", "javascript")]
    [InlineData(".json", "json")]
    [InlineData(".csx", "csharp")]
    [InlineData(".cs", "csharp")]
    [InlineData(".text", "plaintext")]
    public async Task ShouldSetLanaguageAttributeWhenFileExtensionMatchesExpected(
        string directoryToLoadFileExtension,
        string expected
    )
    {
        // Given
        var nodePath = new List<string> { "node-path" };
        var rootDirectoryFullName = "root-directory-full-name";
        var directoryToLoadName = "directory-to-load-name";
        var directoryToLoadFullName = "directory-to-load-full-name";
        var nodeType = "node-type";

        var directoryToLoadFileName = $"directory-to-load-file-name";
        var directoryToLoadFileFullName = "directory-to-load-file-full-name";

        var request = new QueryForEditorNodeFromPath(
            nodePath,
            rootDirectoryFullName,
            directoryToLoadFullName,
            nodeType
        );

        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(
            mock => mock.Send(
                new GetDirectoryInfo(
                    directoryToLoadFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardDirectoryInfo(
                directoryToLoadName,
                directoryToLoadFullName,
                rootDirectoryFullName
            )
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new DoesDirectoryExist(
                    directoryToLoadFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            true
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new GetListOfFilesFromDirectory(
                    directoryToLoadFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new List<StandardFileInfo>
            {
                new StandardFileInfo(
                    directoryToLoadFileName,
                    directoryToLoadFullName,
                    directoryToLoadFileFullName,
                    directoryToLoadFileExtension
                )
            }
        );

        // When
        var handler = new QueryForEditorNodeFromPathHandler(
            mediatorMock.Object
        );
        var actual = await handler.Handle(
            request,
            CancellationToken.None
        );

        // Then
        // Check the First Child on Root is expected
        actual.Children
            .First()
            .Should().NotBeNull();
        actual.Children
            .First()
            .Properties
            .Should().ContainKey("language")
            .And
            .ContainValue(
                expected
            );
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public async Task ShouldSetNodeTypeToDefaultWhenRequestNodeTypeIsNullOrEmpty(
        string nodeType
    )
    {
        // Given
        var expected = QueryForEditorNodeFromPathHandler.DEFAULT_NODE_TYPE;
        var nodePath = new List<string> { "node-path" };
        var rootDirectoryFullName = "root-directory-full-name";
        var directoryToLoadName = "directory-to-load-name";
        var directoryToLoadFullName = "directory-to-load-full-name";

        var directoryToLoadFileName = $"directory-to-load-file-name";
        var directoryToLoadFileFullName = "directory-to-load-file-full-name";
        var directoryToLoadFileExtension = "directory-to-load-file-extension";

        var request = new QueryForEditorNodeFromPath(
            nodePath,
            rootDirectoryFullName,
            directoryToLoadFullName,
            nodeType
        );

        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(
            mock => mock.Send(
                new GetDirectoryInfo(
                    directoryToLoadFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardDirectoryInfo(
                directoryToLoadName,
                directoryToLoadFullName,
                rootDirectoryFullName
            )
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new DoesDirectoryExist(
                    directoryToLoadFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            true
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new GetListOfFilesFromDirectory(
                    directoryToLoadFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new List<StandardFileInfo>
            {
                new StandardFileInfo(
                    directoryToLoadFileName,
                    directoryToLoadFullName,
                    directoryToLoadFileFullName,
                    directoryToLoadFileExtension
                )
            }
        );

        // When
        var handler = new QueryForEditorNodeFromPathHandler(
            mediatorMock.Object
        );
        var actual = await handler.Handle(
            request,
            CancellationToken.None
        );

        // Then
        // Check the First Child on Root is expected
        actual.Children
            .First()
            .Should().NotBeNull();
        actual.Children
            .First()
            .Type
            .Should().Be(expected);
    }

    [Fact]
    public async Task ShouldContainNoChildrenWhenDirectoryDoesNotExist()
    {
        // Given
        var nodePath = new List<string> { "node-path" };
        var rootDirectoryFullName = "root-directory-full-name";
        var directoryToLoadName = "directory-to-load-name";
        var directoryToLoadFullName = "directory-to-load-full-name";
        var nodeType = "node-type";
        var request = new QueryForEditorNodeFromPath(
            nodePath,
            rootDirectoryFullName,
            directoryToLoadFullName,
            nodeType
        );

        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(
            mock => mock.Send(
                new GetDirectoryInfo(
                    directoryToLoadFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardDirectoryInfo(
                directoryToLoadName,
                directoryToLoadFullName,
                rootDirectoryFullName
            )
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new DoesDirectoryExist(
                    directoryToLoadFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            false
        );

        // When
        var handler = new QueryForEditorNodeFromPathHandler(
            mediatorMock.Object
        );
        var actual = await handler.Handle(
            request,
            CancellationToken.None
        );

        // Then
        actual.Children
            .Should()
            .BeEmpty();

    }
}
