namespace EventHorizon.Zone.System.Particle.Tests.Query;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using EventHorizon.Zone.System.Particle.Query;
using EventHorizon.Zone.System.Particle.Model.Template;
using EventHorizon.Zone.System.Particle.State;

using Moq;

using Xunit;

public class QueryForAllParticleTemplatesHandlerTests
{
    [Fact]
    public async Task TestShouldReturnAllFromRepositoryWhenHandleIsCalled()
    {
        // Given
        var expected = new ParticleTemplate
        {
            Id = "particle-template"
        };

        var particleTemplateRepositoryMock =
            new Mock<ParticleTemplateRepository>();

        particleTemplateRepositoryMock
            .Setup(mock => mock.All())
            .Returns(
                new List<ParticleTemplate> { expected }
            );

        // When
        var handler =
            new QueryForAllParticleTemplatesHandler(
                particleTemplateRepositoryMock.Object
            );
        var actual = await handler.Handle(
            new QueryForAllParticleTemplates(),
            CancellationToken.None
        );

        // Then
        Assert.Collection(
            actual,
            actualTemplate =>
                Assert.Equal(expected, actualTemplate)
        );
    }
}
