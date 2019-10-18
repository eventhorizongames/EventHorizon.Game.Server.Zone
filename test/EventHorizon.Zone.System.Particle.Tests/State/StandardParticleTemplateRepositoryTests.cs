using System.Linq;
using EventHorizon.Zone.System.Particle.Model.Template;
using EventHorizon.Zone.System.Particle.State;
using Xunit;

namespace EventHorizon.Zone.System.Particle.Tests.State
{
    public class StandardParticleTemplateRepositoryTests
    {
        [Fact]
        public void TestShouldReturnCollectionOfAllAddTemplatesWhenRequestedFromAll()
        {
            // Given
            var expected1 = new ParticleTemplate
            {
                Id = "1_template"
            };
            var expected2 = new ParticleTemplate
            {
                Id = "2_template"
            };
            var expected3 = new ParticleTemplate
            {
                Id = "3_template"
            };
            
            // When
            var repository = new StandardParticleTemplateRepository();
            repository.Add(
                expected1.Id,
                expected1
            );
            repository.Add(
                expected2.Id,
                expected2
            );
            repository.Add(
                expected3.Id,
                expected3
            );
            var actual = repository.All();
            
            // Then
            Assert.Collection(
                actual.OrderBy(
                    template => template.Id
                ),
                template => Assert.Equal(expected1, template),
                template => Assert.Equal(expected2, template),
                template => Assert.Equal(expected3, template)
            );
        }
    }
}