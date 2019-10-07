using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Entity.Client.Movement;
using EventHorizon.Zone.Core.Events.Client.Actions;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.Core.Entity.Tests.Client.Movement
{
    public class ClientActionEntityClientStoppingHandlerTests
    {
        [Fact]
        public async Task TestShouldShowThatTheApiForClientChangedEventHandlersDoesNotChange()
        {
            // Given
            var errorMessage = "Should of been able to correctly Handle the inputEvent.";
            var inputEvent = new ClientActionClientEntityStoppingToAllEvent();

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ClientActionEntityClientStoppingHandler(
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