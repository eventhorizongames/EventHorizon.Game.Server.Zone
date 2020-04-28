namespace EventHorizon.Zone.System.Editor.Tests.State
{
    using EventHorizon.Zone.System.Editor.Events.State;
    using EventHorizon.Zone.System.Editor.State;
    using FluentAssertions;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class GetEditorStateHandlerTests
    {
        [Fact]
        public async Task ShouldCallFillEditorNodeStateWhenRequestIsHandled()
        {
            // Given
            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new GetEditorStateHandler(
                mediatorMock.Object
            );
            var actual = await handler.Handle(
                new GetEditorState(),
                CancellationToken.None
            );

            // Then
            actual.Root
                .Should().NotBeNull();
        }
    }
}
