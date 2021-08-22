namespace EventHorizon.Zone.System.Particle.Tests.Add
{
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.System.Particle.Add;
    using EventHorizon.Zone.System.Particle.Events.Add;
    using EventHorizon.Zone.System.Particle.Model.Template;
    using EventHorizon.Zone.System.Particle.State;

    using Moq;

    using Xunit;

    public class AddParticleTemplateHandlerTests
    {
        [Fact]
        public async Task TestShouldAddTemplateToRepositoryWhenHandleIsCalled()
        {
            // Given
            var expectedId = "expected-id";
            var expectedTemplate = new ParticleTemplate();


            var particleTemplateRepositoryMock = new Mock<ParticleTemplateRepository>();

            // When
            var handler = new AddParticleTemplateHandler(
                particleTemplateRepositoryMock.Object
            );
            await handler.Handle(
                new AddParticleTemplateEvent(
                    expectedId,
                    expectedTemplate
                ),
                CancellationToken.None
            );

            // Then
            particleTemplateRepositoryMock.Verify(
                mock => mock.Add(
                    expectedId,
                    expectedTemplate
                )
            );
        }
    }
}
