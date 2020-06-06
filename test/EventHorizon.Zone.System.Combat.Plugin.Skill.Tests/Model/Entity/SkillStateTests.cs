namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Model.Entity
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;
    using FluentAssertions;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Text;
    using Xunit;

    public class SkillStateTests
    {
        [Fact]
        public void ShouldReturnSkillWithDefaultDateTimeWhenSkillIsNotInState()
        {
            // Given
            var skillId = "skill-id";

            var expected = new SkillStateDetails
            {
                Id = skillId,
            };

            // When
            var map = new SkillState();
            var actual = map.GetSkill(
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
            var state = new SkillState
            {
                SkillMap = new SkillStateMap
                {
                    List = new List<SkillStateDetails>
                    {
                        expected,
                    }
                }
            };
            var actual = state.GetSkill(
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
            var state = new SkillState
            {
                SkillMap = new SkillStateMap
                {
                    List = new List<SkillStateDetails>
                    {
                        new SkillStateDetails
                        {
                            Id = skillId,
                            CooldownFinishes = DateTime.Now.AddDays(100),
                        },
                    }
                }
            };
            state = state.SetSkill(
                expected
            );
            var actual = state.GetSkill(
                skillId
            );

            // Then
            actual.Should().Be(expected);
        }
    }
}
