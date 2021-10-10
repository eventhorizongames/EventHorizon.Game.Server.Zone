namespace EventHorizon.Zone.System.Player.Plugin.Editor.Tests.Fill
{
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Editor.Events;
    using EventHorizon.Zone.System.Editor.Events.Node;
    using EventHorizon.Zone.System.Editor.Model;
    using EventHorizon.Zone.System.Player.Plugin.Editor.Fill;

    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class FillPlayerEditorNodeStateHandlerTests
    {
        [Fact]
        public async Task ShouldAppendCreatedNodeToPassedInNotification_old()
        {
            // Given
            var expected = "Player";
            var appDataPath = "app_data-path";
            var playerPath = Path.Combine(
                appDataPath,
                "Player"
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

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
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
            var handler = new FillPlayerEditorNodeStateHandler(
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
        public async Task ShouldSetPropertyDeleteOfFalseWhenNodeContainsAnyChildren()
        {
            // Given
            var appDataPath = "app_data-path";
            var playerPath = Path.Combine(
                appDataPath,
                "Player"
            );
            var editorStateMock = new Mock<IEditorNodeList>();
            var nodeState = new FillEditorNodeState(
                editorStateMock.Object
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var editorNodeMock = new Mock<IEditorNode>();
            var editorNodeChildMock = new Mock<IEditorNode>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

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
                    editorNodeChildMock.Object
                }
            );


            // When
            var handler = new FillPlayerEditorNodeStateHandler(
                mediatorMock.Object,
                serverInfoMock.Object
            );
            await handler.Handle(
                nodeState,
                CancellationToken.None
            );

            // Then
            editorNodeChildMock.Verify(
                mock => mock.AddProperty(
                    EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY,
                    false
                )
            );
        }
    }
}
