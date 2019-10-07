using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using Xunit;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.State
{
    public class InMemoryAgentBehaviorScriptRepositoryTests
    {
        [Fact]
        public void ShouldReturnExpectedBehaviorScript()
        {
            // Given
            var expectedId = "script-id";
            var expectedBehaviorScript = new BehaviorScript(
                expectedId,
                null
            );

            // When
            var inMemoryAgentBehaviorScriptRepository = new InMemoryActorBehaviorScriptRepository();
            inMemoryAgentBehaviorScriptRepository.Clear();
            inMemoryAgentBehaviorScriptRepository.Add(
                expectedBehaviorScript
            );
            var actual = inMemoryAgentBehaviorScriptRepository.Find(
                "script-id"
            );

            // Then
            Assert.Equal(
                expectedBehaviorScript,
                actual
            );
        }
        [Fact]
        public void ShouldClearOutInternalStateWhenClearIsCalled()
        {
            // Given
            var expectedId = "script-id";
            var expectedBehaviorScript = new BehaviorScript(
                expectedId,
                null
            );

            // When
            var inMemoryAgentBehaviorScriptRepository = new InMemoryActorBehaviorScriptRepository();
            inMemoryAgentBehaviorScriptRepository.Add(
                expectedBehaviorScript
            );
            var firstFind = inMemoryAgentBehaviorScriptRepository.Find(
                "script-id"
            );

            // Then
            Assert.Equal(
                expectedBehaviorScript,
                firstFind
            );
            // Then Call Clear
            inMemoryAgentBehaviorScriptRepository.Clear();
            var secondFind = inMemoryAgentBehaviorScriptRepository.Find(
                "script-id"
            );
            Assert.NotEqual(
                expectedBehaviorScript,
                secondFind
            );
        }

        [Fact]
        public void ShouldReturnDefaultScriptOnNotFound()
        {
            // Given
            var expectedBehaviorScript = new BehaviorScript();

            // When
            var inMemoryAgentBehaviorScriptRepository = new InMemoryActorBehaviorScriptRepository();
            inMemoryAgentBehaviorScriptRepository.Clear();
            var actual = inMemoryAgentBehaviorScriptRepository.Find(
                "script-id"
            );

            // Then
            Assert.Equal(
                expectedBehaviorScript,
                actual
            );
        }
    }
}