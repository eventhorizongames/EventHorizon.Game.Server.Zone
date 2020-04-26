using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Lifetime;
using EventHorizon.Zone.Core.Lifetime.State;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.Core.Tests.Lifetime
{
    public class FinishServerStartCommandHandlerTests
    {
        [Fact]
        public async Task ShouldReturnTrueWhenRequestIsHandled()
        {
            // Given
            var expected = true;

            var mediatorMock = new Mock<IMediator>();
            var serverLifetimeState = new StandardServerLifetimeState();

            // When
            var handler = new FinishServerStartCommandHandler(
                mediatorMock.Object,
                serverLifetimeState
            );
            var actual = await handler.Handle(
                new FinishServerStartCommand(),
                CancellationToken.None
            );


            // Then
            actual.Should()
                .Be(
                    expected
                );
            mediatorMock.Verify(
                mock => mock.Publish(
                    new ServerFinishedStartingEvent(),
                    CancellationToken.None
                )
            );
        }
    }
}