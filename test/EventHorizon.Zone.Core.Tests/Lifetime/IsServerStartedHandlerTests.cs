namespace EventHorizon.Zone.Core.Tests.Lifetime
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.Core.Lifetime;
    using EventHorizon.Zone.Core.Lifetime.State;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class IsServerStartedHandlerTests
    {
        [Fact]
        public async Task ShouldReturnIsServerStartedValueFromServerLifetimeStateWhenRequestIsHandled()
        {
            // Given
            var expected = true;

            var serverLifetimeStateMock = new Mock<ServerLifetimeState>();

            serverLifetimeStateMock.Setup(
                mock => mock.IsServerStarted()
            ).Returns(
                expected
            );

            // When
            var handler = new IsServerStartedHandler(
                serverLifetimeStateMock.Object
            );
            var actual = await handler.Handle(
                new IsServerStarted(),
                CancellationToken.None
            );

            // Then
            actual.Should()
                .Be(
                    expected
                );
        }
    }
}
