namespace EventHorizon.Zone.System.Particle.Tests.State
{
    using EventHorizon.Zone.System.Particle.Model.Template;
    using EventHorizon.Zone.System.Particle.State;

    using FluentAssertions;

    using global::System.Collections.Generic;

    using Xunit;

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
            actual.Should().BeEquivalentTo(
                new List<ParticleTemplate>
                {
                    expected1,
                    expected2,
                    expected3,
                }
            );
        }

        [Fact]
        public void ShouldRemoveAllAddedParticleTempaltesWhenClearIsCalled()
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
            repository.All()
                .Should().HaveCount(3);
            repository.Clear();
            var actual = repository.All();

            // Then
            actual.Should().BeEmpty();
        }
    }
}
