using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Register;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Register
{
    public class RegisterActorWithBehaviorTreeUpdateHandlerTests
    {
        [Fact]
        public async Task ShouldRegisterActorIdWithTheTreeOfTheEvent()
        {
            // Given
            var actorId = 123L;
            var treeId = "behavior-tree-id";

            var actorBehaviorTreeRepositoryMock = new Mock<ActorBehaviorTreeRepository>();
            actorBehaviorTreeRepositoryMock.Setup(
                repository => repository.RegisterActorToTree(
                    actorId,
                    treeId
                )
            );

            // When
            var registerActorWithBehaviorTreeUpdateHandler = new RegisterActorWithBehaviorTreeUpdateHandler(
                actorBehaviorTreeRepositoryMock.Object
            );
            await registerActorWithBehaviorTreeUpdateHandler.Handle(
                new RegisterActorWithBehaviorTreeUpdate(
                    actorId,
                    treeId
                ),
                CancellationToken.None
            );

            // Then
            actorBehaviorTreeRepositoryMock.Verify(
                repository => repository.RegisterActorToTree(
                    actorId,
                    treeId
                )
            );
        }
    }
}