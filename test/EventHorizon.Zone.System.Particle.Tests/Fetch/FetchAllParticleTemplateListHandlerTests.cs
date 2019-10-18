using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Particle.Fetch;
using EventHorizon.Zone.System.Particle.Model.Template;
using EventHorizon.Zone.System.Particle.State;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Particle.Tests.Fetch
{
    public class FetchAllParticleTemplateListHandlerTests
    {
        [Fact]
        public async Task TestShouldReturnAllFromRepositoryWhenHandleIsCalled()
        {
            // Given
            var expected = new ParticleTemplate
            {
                Id = "particle-template"
            };

            var particleTemplateRepositoryMock = new Mock<ParticleTemplateRepository>();

            particleTemplateRepositoryMock.Setup(
                mock => mock.All()
            ).Returns(
                new List<ParticleTemplate>
                {
                    expected
                }
            );

            // When
            var handler = new FetchAllParticleTemplateListHandler(
                particleTemplateRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new FetchAllParticleTemplateListEvent(),
                CancellationToken.None
            );

            // Then
            Assert.Collection(
                actual,
                actualTemplate => Assert.Equal(expected, actualTemplate)
            );
        }
    }
}