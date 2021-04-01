namespace EventHorizon.Zone.System.Wizard.Plugin.Editor.Tests.Fill
{
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Editor.Events;
    using EventHorizon.Zone.System.Editor.Events.Node;
    using EventHorizon.Zone.System.Editor.Model;
    using EventHorizon.Zone.System.Wizard.Plugin.Editor.Fill;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class FillWizardEditorNodeStateHandlerTests
    {
        [Fact]
        public async Task ShouldAppendCreatedNodeToPassedInNotification()
        {
            // Given
            var expected = "Server";
            var serverPath = "server-path";
            var wizardsPath = Path.Combine(
                serverPath,
                "Wizards"
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
                mock => mock.ServerPath
            ).Returns(
                serverPath
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
            var handler = new FillWizardEditorNodeStateHandler(
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
                    It.Is<IEditorNode>(
                        a => a.Name == expected
                    )
                )
            );
        }

        [Fact]
        public async Task ShouldAppendFoundScriptNodeToRootNode()
        {
            // Given
            var expected = 1;
            var serverPath = "server-path";
            var wizardsPath = Path.Combine(
                serverPath,
                "Wizards"
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
                mock => mock.ServerPath
            ).Returns(
                serverPath
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
            var handler = new FillWizardEditorNodeStateHandler(
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
                    It.Is<IEditorNode>(
                        a => a.Children.Count == expected
                            && a.Children.First() == editorNodeMock.Object
                    )
                )
            );
        }

        [Fact]
        public async Task ShouldSetPropertyDeleteOfFalseWhenNodeContainsAnyChildren()
        {
            // Given
            var serverPath = "server-path";
            var wizardsPath = Path.Combine(
                serverPath,
                "Wizards"
            );
            var editorStateMock = new Mock<IEditorNodeList>();
            var nodeState = new FillEditorNodeState(
                editorStateMock.Object
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var editorNodeMock = new Mock<IEditorNode>();
            var editorNodeChildMock = new Mock<IEditorNode>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<QueryForEditorNodeFromPath>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                editorNodeMock.Object
            );

            serverInfoMock.Setup(
                mock => mock.ServerPath
            ).Returns(
                serverPath
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
            var handler = new FillWizardEditorNodeStateHandler(
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
                    EditorNodePropertySupportKeys.SUPPORT_DELETE_KEY,
                    false
                )
            );
        }
    }
}
