using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.Agent.Ai.Script;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.Script;
using EventHorizon.Zone.System.Agent.Behavior.Script.Run;
using Moq;
using Xunit;
using static EventHorizon.Zone.System.Agent.Behavior.Script.Run.RunBehaviorScript;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.Script.Run
{
    public class RunBehaviorScriptHandlerTests
    {
        [Fact]
        public async Task ShouldRunExpectedScriptFromRepository()
        {
            // Given
            var expected = BehaviorNodeStatus.SUCCESS;
            var scriptId = "script-id";
            var actor = new DefaultEntity();
            var behaviorScript = BehaviorScript.CreateScript(
                scriptId,
                "return new BehaviorScriptResponse(BehaviorNodeStatus.SUCCESS);"
            );

            var actorBehaviorScriptRepositoryMock = new Mock<ActorBehaviorScriptRepository>();
            var scriptServicesMock = new Mock<IScriptServices>();

            actorBehaviorScriptRepositoryMock.Setup(
                repository => repository.Find(
                    scriptId
                )
            ).Returns(
                behaviorScript
            );

            // When
            var runBehaviorScriptHandler = new RunBehaviorScriptHandler(
                actorBehaviorScriptRepositoryMock.Object,
                scriptServicesMock.Object
            );
            var actual = await runBehaviorScriptHandler.Handle(
                new RunBehaviorScript(
                    actor,
                    scriptId
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual.Status
            );
        }
    }
}