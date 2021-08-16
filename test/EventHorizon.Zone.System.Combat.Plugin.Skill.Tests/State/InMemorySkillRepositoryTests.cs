namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.State
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.State;

    using FluentAssertions;

    using global::System.Collections.Generic;

    using Xunit;

    public class InMemorySkillRepositoryTests
    {
        [Fact]
        public void ShouldReturnExpectedWhenSetInRepository()
        {
            // Given
            var skillId = "skill-id";
            var skill = new SkillInstance
            {
                Id = skillId,
            };
            var expected = skill;

            // When
            var repository = new InMemorySkillRepository();
            repository.Set(
                skill
            );
            var actual = repository.Find(
                skillId
            );

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnAllSetWhenAllIsCalled()
        {
            // Given
            var skill1 = new SkillInstance
            {
                Id = "skill-id-1",
            };
            var skill2 = new SkillInstance
            {
                Id = "skill-id-2",
            };
            var expected = new List<SkillInstance>
            {
                skill1,
                skill2,
            };

            // When
            var repository = new InMemorySkillRepository();
            repository.Set(
                skill1
            );
            repository.Set(
                skill2
            );
            var actual = repository.All();

            // Then
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldOverrideBasedOnIdWhenSettingSkill()
        {
            // Given
            var skillId = "skill-id";
            var skill1 = new SkillInstance
            {
                Id = "skill-id",
                Name = "skill-1-name",
            };
            var skill2 = new SkillInstance
            {
                Id = "skill-id",
                Name = "skill-2-name",
            };
            var expected = skill2;

            // When
            var repository = new InMemorySkillRepository();
            repository.Set(
                skill1
            );
            repository.Set(
                skill2
            );
            var actual = repository.Find(
                skillId
            );

            // Then
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
