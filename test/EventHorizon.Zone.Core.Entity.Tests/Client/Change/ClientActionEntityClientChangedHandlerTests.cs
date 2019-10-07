using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Entity.Client.Change;
using EventHorizon.Zone.Core.Events.Client.Actions;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.Core.Entity.Tests.Client.Change
{
    public class ClientActionEntityClientChangedHandlerTests
    {
        [Fact]
        public async Task TestShouldShowThatTheApiForClientChangedEventHandlersDoesNotChange()
        {
            // Given
            var errorMessage = "Should of been able to correctly Handle the inputEvent.";
            var inputEvent = new ClientActionEntityClientChangedToAllEvent();

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ClientActionEntityClientChangedHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                inputEvent,
                CancellationToken.None
            );

            // Then
            Assert.True(
                true,
                errorMessage
            );
        }
    }
}