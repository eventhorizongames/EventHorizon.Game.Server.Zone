namespace EventHorizon.Zone.Core.Map.Plugin.Editor.Tests.Fill
{
    using EventHorizon.Zone.Core.Map.Plugin.Editor.Fill;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Editor.Events;
    using EventHorizon.Zone.System.Editor.Events.Node;
    using EventHorizon.Zone.System.Editor.Model;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class FillCoreMapEditorNodeStateHandlerTests
    {
        [Fact]
        public async Task ShouldAppendCreatedNodeToPassedInNotification()
        {
            // Given
            var coreMapPathRoot = "root-path";
            var coreMapPath = Path.Combine(
                coreMapPathRoot,
                "map-path"
            );
            var editorStateMock = new Mock<IEditorNodeList>();
            var nodeState = new FillEditorNodeState(
                editorStateMock.Object
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var editorNodeMock = new Mock<IEditorNode>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<QueryForEditorNodeFromPath>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                editorNodeMock.Object
            );

            editorNodeMock.Setup(
                mock => mock.AddProperty(
                    EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY,
                    false
                )
            ).Returns(
                editorNodeMock.Object
            );
            editorNodeMock.Setup(
                mock => mock.Children
            ).Returns(
                new List<IEditorNode> { }
            );

            // When
            var handler = new FillCoreMapEditorNodeStateHandler(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            await handler.Handle(
                nodeState,
                CancellationToken.None
            );

            // Then
            editorStateMock.Verify(
                mock => mock.AddNode(
                    editorNodeMock.Object
                )
            );
        }

        [Fact]
        public async Task ShouldAddContextPropertyOfFalseWhenAnyChildrenAreFound()
        {
            // Given
            var coreMapPathRoot = "root-path";
            var coreMapPath = Path.Combine(
                coreMapPathRoot,
                "map-path"
            );
            var editorStateMock = new Mock<IEditorNodeList>();
            var nodeState = new FillEditorNodeState(
                editorStateMock.Object
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var editorNodeMock = new Mock<IEditorNode>();
            var childEditorNode = new Mock<IEditorNode>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<QueryForEditorNodeFromPath>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                editorNodeMock.Object
            );

            editorNodeMock.Setup(
                mock => mock.AddProperty(
                    EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY,
                    false
                )
            ).Returns(
                editorNodeMock.Object
            );
            editorNodeMock.Setup(
                mock => mock.Children
            ).Returns(
                new List<IEditorNode>
                {
                    childEditorNode.Object,
                }
            );

            // When
            var handler = new FillCoreMapEditorNodeStateHandler(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            await handler.Handle(
                nodeState,
                CancellationToken.None
            );

            // Then
            childEditorNode.Verify(
                mock => mock.AddProperty(
                    EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY,
                    false
                )
            );
        }
    }
}
