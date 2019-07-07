using EventHorizon.Game.Server.Zone.Agent.Move.Repository.Impl;
using Xunit.Sdk;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Repository
{
    public class CleanupMoveAgentRepositoryAttribute : BeforeAfterTestAttribute
    {
        public override void Before(System.Reflection.MethodInfo methodUnderTest)
        {
            var moveAgentRepository = new MoveAgentRepository();
            while (moveAgentRepository.Dequeue(out _)) ;
        }
    }
}