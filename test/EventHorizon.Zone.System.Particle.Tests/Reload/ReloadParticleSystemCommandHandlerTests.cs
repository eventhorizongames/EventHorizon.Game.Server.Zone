namespace EventHorizon.Zone.System.Particle.Tests.Reload
{
    using EventHorizon.Zone.System.Particle.Load;
    using EventHorizon.Zone.System.Particle.Reload;
    using EventHorizon.Zone.System.Particle.State;

    using FluentAssertions;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;


    public class ReloadParticleSystemCommandHandlerTests
    {
        [Fact]
        public async Task ShouldClearAndLoadParticleSystemWhenCommandIsHandled()
        {
            // Given
            var expected = new LoadParticleSystemEvent();

            var mediatorMock = new Mock<IMediator>();
            var repositoryMock = new Mock<ParticleTemplateRepository>();

            // When
            var handler = new ReloadParticleSystemCommandHandler(
                mediatorMock.Object,
                repositoryMock.Object
            );
            var actual = await handler.Handle(
                new ReloadParticleSystemCommand(

                ),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();

            repositoryMock.Verify(
                mock => mock.Clear()
            );
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }
}
