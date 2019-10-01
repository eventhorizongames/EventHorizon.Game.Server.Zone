using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script.Run;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Moq;
using Xunit;
using static EventHorizon.Zone.System.Agent.Plugin.Behavior.Script.Run.RunBehaviorScript;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Script.Run
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
            var scriptServicesMock = new Mock<ServerScriptServices>();

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