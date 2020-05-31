namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Model.Entity
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;
    using FluentAssertions;
    using global::System;
    using Xunit;

    public class SkillStateDetailsTests
    {
        [Fact]
        public void ShouldKeepTrackOfCooldownFinsihesWhenCreated()
        {
            // Given
            var expected = DateTime.UtcNow;

            // When
            var actual = new SkillStateDetails
            {
                Id = "id",
                CooldownFinishes = expected,
            };

            // Then
            actual.CooldownFinishes
                .Should().Be(expected);
        }
    }
}
