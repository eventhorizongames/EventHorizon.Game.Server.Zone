namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Model
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using FluentAssertions;
    using global::System.Threading.Tasks;
    using Xunit;

    public class SkillEffectScriptResponseTests
    {
        [Fact]
        public void ShouldDefaultSuccessToTrueWhenCreated()
        {
            // Given
            var expected = true;

            // When
            var actual = new SkillEffectScriptResponse();

            // Then
            actual.Success
                .Should().Be(expected);
        }

        [Fact]
        public void ShouldDefaultMessageToEmptyStringWhenCreated()
        {
            // Given
            var expected = string.Empty;

            // When
            var actual = new SkillEffectScriptResponse();

            // Then
            actual.Message
                .Should().Be(expected);
        }
    }
}
