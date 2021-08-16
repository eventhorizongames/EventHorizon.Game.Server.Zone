namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Model
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

    using FluentAssertions;

    using global::System.Collections.Generic;
    using global::System.Threading.Tasks;

    using Xunit;

    public class SkillEffectTests
    {
        [Fact]
        public void ShouldContainExpectedWhenCreated()
        {
            // Given
            var comment = "comment";
            var effect = "effect";
            var duration = 123L;
            var data = new Dictionary<string, object>();
            var validatorList = new List<SkillValidator>();
            var next = new List<SkillEffect>();
            var failedList = new List<SkillEffect>();

            var expectedComment = comment;
            var expectedEffect = effect;
            var expectedDuration = duration;
            var expectedData = data;
            var expectedValidatorList = validatorList;
            var expectedNext = next;
            var expectedFailedList = failedList;

            // When
            var actual = new SkillEffect
            {
                Comment = comment,
                Effect = effect,
                Duration = duration,
                Data = data,
                ValidatorList = validatorList,
                Next = next,
                FailedList = failedList,
            };

            // Then
            actual.Comment
                .Should().Be(expectedComment);
            actual.Effect
                .Should().Be(expectedEffect);
            actual.Duration
                .Should().Be(expectedDuration);
            actual.Data
                .Should().BeEquivalentTo(expectedData);
            actual.ValidatorList
                .Should().BeEquivalentTo(expectedValidatorList);
            actual.Next
                .Should().BeEquivalentTo(expectedNext);
            actual.FailedList
                .Should().BeEquivalentTo(expectedFailedList);

        }
    }
}
