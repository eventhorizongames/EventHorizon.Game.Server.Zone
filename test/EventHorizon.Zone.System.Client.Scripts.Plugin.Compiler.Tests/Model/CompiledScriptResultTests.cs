namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Tests.Model
{
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Model;
    using FluentAssertions;
    using Xunit;

    public class CompiledScriptResultTests
    {
        [Fact]
        public void ShouldReturnFilledHashAndScriptAssemblyWhenSuccessConstructorIsUsed()
        {
            // Given
            var expectedSuccess = true;
            var expectedHash = "hash";
            var expectedScriptAssembly = "script-assembly";
            var expectedErrorCode = string.Empty;

            // When
            var actual = new CompiledScriptResult(
                expectedHash,
                expectedScriptAssembly
            );

            // Then
            actual.Success.Should().Be(
                expectedSuccess
            );
            actual.ErrorCode.Should().Be(
                expectedErrorCode
            );
            actual.Hash.Should().Be(
                expectedHash
            );
            actual.ScriptAssembly.Should().Be(
                expectedScriptAssembly
            );
        }

        [Fact]
        public void ShouldReturnNotFilledHashAndScriptAssemblyWhenFailedConstructorIsUsed()
        {
            // Given
            var expectedSuccess = false;
            var expectedHash = string.Empty;
            var expectedScriptAssembly = string.Empty;
            var expectedErrorCode = "error-code";

            // When
            var actual = new CompiledScriptResult(
                false,
                expectedErrorCode
            );

            // Then
            actual.Success.Should().Be(
                expectedSuccess
            );
            actual.ErrorCode.Should().Be(
                expectedErrorCode
            );
            actual.Hash.Should().Be(
                expectedHash
            );
            actual.ScriptAssembly.Should().Be(
                expectedScriptAssembly
            );
        }
    }
}
