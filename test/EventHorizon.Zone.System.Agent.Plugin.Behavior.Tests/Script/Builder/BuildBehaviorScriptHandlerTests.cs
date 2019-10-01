using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script.Builder;
using Xunit;
using static EventHorizon.Zone.System.Agent.Plugin.Behavior.Script.Builder.BuildBehaviorScript;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Script.Builder
{
    public class BuildBehaviorScriptHandlerTests
    {
        [Fact]
        public async Task ShouldReturnBuiltScript()
        {
            // Given
            var expected = "script-id";
            var scriptContent = "";

            // When
            var buildBehaviorScriptHandler = new BuildBehaviorScriptHandler();
            var actual = await buildBehaviorScriptHandler.Handle(
                new BuildBehaviorScript(
                    expected,
                    scriptContent
                ),
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual.Id
            );
        }
    }
}