namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Model
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using FluentAssertions;
    using global::System.Collections.Generic;
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

        [Fact]
        public void ShouldHaveEmptyActionListWhenCreatedByNewStaticMethod()
        {
            // Given
            var expected = new List<ClientSkillAction>();

            // When
            var actual = SkillEffectScriptResponse.New();

            // Then
            actual.ActionList
                .Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldContainExpectedActionListWhenUsingAddMethod()
        {
            // Given
            var clientActionMock = new ClientSkillActionMock();
            var expected = new List<ClientSkillAction>
            {
                clientActionMock
            };

            // When
            var actual = SkillEffectScriptResponse
                .New()
                .Add(
                    clientActionMock
                );

            // Then
            actual.ActionList
                .Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldContainExpectedActionListWhenUsingAddMethodWhenNotUsingNew()
        {
            // Given
            var clientActionMock = new ClientSkillActionMock();
            var expected = new List<ClientSkillAction>
            {
                clientActionMock
            };

            // When
            var actual = new SkillEffectScriptResponse()
                .Add(
                    clientActionMock
                );

            // Then
            actual.ActionList
                .Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldContainExpectedStateWhenUsingSetMethod()
        {
            // Given
            var stateKey = "state-key";
            var stateValue = "state-value";
            var expected = new Dictionary<string, object>
            {
                { stateKey, stateValue }
            };

            // When
            var actual = SkillEffectScriptResponse
                .New()
                .Set(
                    stateKey,
                    stateValue
                );

            // Then
            actual.State
                .Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldContainExpectedStateWhenUsingSetMethodWhenNotUsingNew()
        {
            // Given
            var stateKey = "state-key";
            var stateValue = "state-value";
            var expected = new Dictionary<string, object>
            {
                { stateKey, stateValue }
            };

            // When
            var actual = new SkillEffectScriptResponse()
                .Set(
                    stateKey,
                    stateValue
                );

            // Then
            actual.State
                .Should().BeEquivalentTo(expected);
        }

        public struct ClientSkillActionMock : ClientSkillAction
        {
            public string Action { get; set; }
            public object Data { get; set; }
        }
    }
}
