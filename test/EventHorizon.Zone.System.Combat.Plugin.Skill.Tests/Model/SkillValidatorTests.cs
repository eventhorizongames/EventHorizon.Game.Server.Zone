namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Model;

using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

using FluentAssertions;

using global::System.Collections.Generic;

using Xunit;

public class SkillValidatorTests
{
    [Fact]
    public void ShouldHaveExpectedDataWhenCreated()
    {
        // Given
        var data = new Dictionary<string, object>();

        var expected = data;

        // When
        var actual = new SkillValidator
        {
            Data = data,
        };

        // Then
        actual.Data
            .Should().BeEquivalentTo(expected);
    }
}
