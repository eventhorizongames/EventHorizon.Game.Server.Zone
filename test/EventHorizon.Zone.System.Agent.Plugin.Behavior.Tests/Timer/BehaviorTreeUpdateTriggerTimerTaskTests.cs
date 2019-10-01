using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Timer;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Update;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Timer
{
    public class BehaviorTreeUpdateTriggerTimerTaskTests
    {
        [Fact]
        public void ShouldHaveExpectedOnRunEvent()
        {
            // Given
            var expected = new RunUpdateOnAllBehaviorTrees();

            // When
            var behaviorTreeUpdateTriggerTimerTask = new BehaviorTreeUpdateTriggerTimerTask();
            
            // Then
            Assert.Equal(
                expected,
                behaviorTreeUpdateTriggerTimerTask.OnRunEvent
            );
        }
        [Fact]
        public void ShouldExpectedPeriod()
        {
            // Given
            var expected = 100;

            // When
            var behaviorTreeUpdateTriggerTimerTask = new BehaviorTreeUpdateTriggerTimerTask();
            
            // Then
            Assert.Equal(
                expected,
                behaviorTreeUpdateTriggerTimerTask.Period
            );
        }
        [Fact]
        public void ShouldExpectedTag()
        {
            // Given
            var expected = "RunUpdateOnAllBehaviorTrees";

            // When
            var behaviorTreeUpdateTriggerTimerTask = new BehaviorTreeUpdateTriggerTimerTask();
            
            // Then
            Assert.Equal(
                expected,
                behaviorTreeUpdateTriggerTimerTask.Tag
            );
        }
    }
}