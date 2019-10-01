using System.Reflection;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
using Xunit.Sdk;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.State
{
    public class CleanupInMemoryActorBehaviorTreeRepositoryAttribute : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            var actorBehaviorTreeRepository = new InMemoryActorBehaviorTreeRepository();
            actorBehaviorTreeRepository.ClearTrees();
        }
    }
}