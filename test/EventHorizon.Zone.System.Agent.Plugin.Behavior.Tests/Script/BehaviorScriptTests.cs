using System;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Script
{
    public class BehaviorScriptTests
    {
        [Fact]
        public void ShouldThrowExceptionWhenCreatingWithNullContent()
        {
            // Given
            var scriptId = "hello-world";
            string scriptContent = null;
            var expectedMessage = $"Exception with {scriptId}";
;
            // When
            Action testAction = () => BehaviorScript.CreateScript(
                scriptId,
                scriptContent
            );

            var actual = Record.Exception(
                testAction
            );

            // Then
            Assert.NotNull(
                actual
            );
            Assert.IsType<InvalidOperationException>(
                actual
            );
            Assert.Equal(
                expectedMessage,
                actual.Message
            );
        }
        [Fact]
        public void ShouldThrowExceptionWhenRunningAnInvalidScript()
        {
            // Given
            var scriptId = "hello-world";
            var scriptContent = "var mediator = Services.Mediator;";
            var expectedMessage = $"Exception with {scriptId}";
;
            // When
            Action testAction = () => BehaviorScript.CreateScript(
                scriptId,
                scriptContent
            ).Run(null, null).GetAwaiter().GetResult();

            var actual = Record.Exception(
                testAction
            );

            // Then
            Assert.NotNull(
                actual
            );
            Assert.IsType<ArgumentNullException>(
                actual
            );
            Assert.Equal(
                expectedMessage,
                actual.Message
            );
        }
    }
}