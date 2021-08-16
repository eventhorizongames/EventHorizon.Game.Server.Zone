namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Model.Entity
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;

    using FluentAssertions;

    using global::System;
    using global::System.Collections.Generic;

    using Xunit;

    public class SkillStateMapTests
    {
        [Fact]
        public void ShouldReturnSkillWithDefaultDateTimeWhenSkillIsNotInList()
        {
            // Given
            var skillId = "skill-id";

            var expected = new SkillStateDetails
            {
                Id = skillId,
            };

            // When
            var map = new SkillStateMap();
            var actual = map.Get(
                skillId
            );

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnExpectedSkillWhenSkillDetailsIsInList()
        {
            // Given
            var skillId = "skill-id";

            var expected = new SkillStateDetails
            {
                Id = skillId,
                CooldownFinishes = DateTime.Now.AddDays(1),
            };

            // When
            var map = new SkillStateMap
            {
                List = new List<SkillStateDetails>
                {
                    expected,
                }
            };
            var actual = map.Get(
                skillId
            );

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReplaceSkillDetailsWhenAlreadyExistingInList()
        {
            // Given
            var skillId = "skill-id";

            var expected = new SkillStateDetails
            {
                Id = skillId,
                CooldownFinishes = DateTime.Now.AddDays(1),
            };

            // When
            var map = new SkillStateMap
            {
                List = new List<SkillStateDetails>
                {
                    new SkillStateDetails
                    {
                        Id = skillId,
                        CooldownFinishes = DateTime.Now.AddDays(100),
                    },
                }
            };
            map = map.Set(
                expected
            );
            var actual = map.Get(
                skillId
            );

            // Then
            actual.Should().Be(expected);
        }
    }
}
