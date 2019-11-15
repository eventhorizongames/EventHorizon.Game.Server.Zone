using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Admin.Plugin.Command.Editor;
using EventHorizon.Zone.System.Editor.Events;
using EventHorizon.Zone.System.Editor.Events.Node;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.Tests.Editor
{
    public class FillAdminCommandsEditorNodeStateHandlerTests
    {
        [Fact]
        public async Task TestShouldAddNodeWhenEditorNodeStateIsPassedIntoHandler()
        {
            // Given
            var adminPath = "Scripts_Admin";
            var expectedNodeName = "Admin";
            var expectedRootDirectoryFullName = adminPath;
            var expectedDirectoryToLoadFullName = Path.Combine(
                adminPath,
                "Commands"
            );

            var editorNodeListMock = new Mock<IEditorNodeList>();
            var editorNodeState = new FillEditorNodeState(
                editorNodeListMock.Object
            );
            var editorNodeMock = new Mock<IEditorNode>();

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<QueryForEditorNodeFromPath>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                editorNodeMock.Object
            );

            serverInfoMock.Setup(
                mock => mock.AdminPath
            ).Returns(
                adminPath
            );

            // When
            var handler = new FillAdminCommandsEditorNodeStateHandler(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            await handler.Handle(
                editorNodeState,
                CancellationToken.None
            );

            // Then
            editorNodeListMock.Verify(
                mock => mock.AddNode(
                    It.Is<IEditorNode>(
                        editorNode =>
                            editorNode.Name == expectedNodeName
                    &&
                    editorNode.Properties.ContainsKey(
                        EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY
                    )
                    &&
                    !((bool)editorNode.Properties[
                        EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY
                    ])
                    &&
                    editorNode.Children.Count == 1
                    )
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<QueryForEditorNodeFromPath>(
                        query => query.NodePath.Contains(
                            expectedNodeName
                        ) && query.RootDirectoryFullName == expectedRootDirectoryFullName
                        && query.DirectoryToLoadFullName == expectedDirectoryToLoadFullName
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}