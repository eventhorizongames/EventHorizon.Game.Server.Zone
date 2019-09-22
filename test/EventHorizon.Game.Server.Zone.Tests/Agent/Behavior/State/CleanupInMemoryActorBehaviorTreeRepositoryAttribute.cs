using EventHorizon.Game.Server.Zone.Agent.Move.Repository.Impl;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
using Xunit.Sdk;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.State
{
    public class CleanupInMemoryActorBehaviorTreeRepositoryAttribute : BeforeAfterTestAttribute
    {
        public override void Before(System.Reflection.MethodInfo methodUnderTest)
        {
            var actorBehaviorTreeRepository = new InMemoryActorBehaviorTreeRepository();
            actorBehaviorTreeRepository.ClearTrees();
        }
    }
}