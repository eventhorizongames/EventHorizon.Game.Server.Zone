namespace EventHorizon.Zone.Core.Tests.Lifetime
{
    using System.Threading;
    using System.Threading.Tasks;

    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.Core.Lifetime;
    using EventHorizon.Zone.Core.Lifetime.State;

    using FluentAssertions;

    using MediatR;

    using Moq;

    using Xunit;

    public class FinishServerStartCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldReturnTrueWhenRequestIsHandled(
            // Given
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<ServerLifetimeState> serverLifetimeState,
            FinishServerStartCommandHandler handler
        )
        {
            var command = new FinishServerStartCommand();
            var expected = true;

            serverLifetimeState.Setup(
                mock => mock.IsServerStarted()
            ).Returns(
                expected
            );

            // When
            var actual = await handler.Handle(
                command,
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
