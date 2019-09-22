using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.UnRegister;
using Moq;
using Xunit;
using static EventHorizon.Zone.System.Agent.Plugin.Behavior.UnRegister.UnRegisterActorWithBehaviorTreeUpdate;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.UnRegister
{
    public class UnRegisterActorWithBehaviorTreeUpdateHandlerTests
    {
        [Fact]
        public async Task ShouldCallActorBehaviorTreeRepositoryWithUnRegisterRequestParameters()
        {
            // Given
            var actorId = 1L;
            var treeId = "tree-id";
            var expectedActorId = actorId;
            var expectedTreeId = treeId;

            var actorBehaviorTreeRepositoryMock = new Mock<ActorBehaviorTreeRepository>();

            // When
            var unRegisterActorWithBehaviorTreeUpdateHandler = new UnRegisterActorWithBehaviorTreeUpdateHandler(
                actorBehaviorTreeRepositoryMock.Object
            );

            await unRegisterActorWithBehaviorTreeUpdateHandler.Handle(
                new UnRegisterActorWithBehaviorTreeUpdate(
                    actorId,
                    treeId
                ),
                CancellationToken.None
            );

            // Then
            actorBehaviorTreeRepositoryMock.Verify(
                repository => repository.UnRegisterActorFromTree(
                    expectedActorId,
                    expectedTreeId
                )
            );
        }
    }
}