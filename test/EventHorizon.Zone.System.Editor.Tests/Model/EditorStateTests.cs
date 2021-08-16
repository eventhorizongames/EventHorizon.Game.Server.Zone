namespace EventHorizon.Zone.System.Editor.Tests.Model
{
    using EventHorizon.Zone.System.Editor.Model;

    using FluentAssertions;

    using global::System.Linq;

    using Moq;

    using Xunit;

    public class EditorStateTests
    {
        [Fact]
        public void ShouldAddNodeToRootWhenStateRootDoesNotAlreadyContainNode()
        {
            // Given
            var nodeName = "node-name";
            var expected = new Mock<IEditorNode>();

            expected.Setup(
                mock => mock.Name
            ).Returns(
                nodeName
            );

            // When
            var editorState = new EditorState();
            editorState.AddNode(expected.Object);

            // Then
            editorState.Root
                .Should().HaveCount(1);
            editorState.Root
                .Should().Contain(expected.Object);
        }

        [Fact]
        public void ShouldAddNodeChildrenWhenNodeWasAlreadyAdded()
        {
            // Given
            var nodeName = "node-name";
            var editorNode1 = new StandardEditorNode(
                nodeName
            );
            var editorNode2ChildName = "editor-node-2-child-name";
            var editorNode2 = new StandardEditorNode(
                nodeName
            ).AddChild(
                new StandardEditorNode(
                    editorNode2ChildName
                )
            );

            // When
            var editorState = new EditorState();
            editorState.AddNode(editorNode1);
            editorState.AddNode(editorNode2);

            // Then
            editorState.Root
                .Should().HaveCount(1);
            editorState.Root
                .First().Children
                .Should().HaveCount(1);
            editorState.Root
                .First().Children
                .First().Name
                .Should().Be(editorNode2ChildName);
        }
    }
}
