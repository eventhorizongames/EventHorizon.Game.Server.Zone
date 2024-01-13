namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Model;

using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

using FluentAssertions;

using global::System.Threading.Tasks;

using Xunit;

public class SkillInstanceTests
{
    [Fact]
    public void ShouldGenerateIdBasedOnPathAndFileName()
    {
        // Given
        var path = "path";
        var fileName = "fileName";

        var expected = "path_fileName";

        // When
        var actual = SkillInstance.GenerateId(
            path,
            fileName
        );

        // Then
        actual.Should().Be(expected);
    }

    [Fact]
    public void ShouldGenerateIdWhenPathIsEmpty()
    {
        // Given
        var path = "";
        var fileName = "fileName";

        var expected = "fileName";

        // When
        var actual = SkillInstance.GenerateId(
            path,
            fileName
        );

        // Then
        actual.Should().Be(expected);
    }

    [Fact]
    public void ShouldAllowForDescriptionWhenSet()
    {
        // Given
        var description = "description";

        var expected = description;

        // When
        var actual = new SkillInstance
        {
            Description = description,
        };


        // Then
        actual.Description
            .Should().Be(expected);
    }
}
