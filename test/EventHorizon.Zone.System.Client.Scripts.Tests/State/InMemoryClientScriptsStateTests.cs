namespace EventHorizon.Zone.System.Client.Scripts.Tests.State;

using EventHorizon.Zone.System.Client.Scripts.State;

using FluentAssertions;

using Xunit;

public class InMemoryClientScriptsStateTests
{
    [Fact]
    public void ShouldUpdateHashAndScriptAssemblyWhenCalledOnState()
    {
        // Given
        var expectedHash = "hash";
        var expectedScriptAssembly = "script-assembly";

        // When
        var state = new InMemoryClientScriptsState();
        state.Hash.Should().BeEmpty();
        state.ScriptAssembly.Should().BeEmpty();
        state.SetAssembly(
            expectedHash,
            expectedScriptAssembly
        );

        // Then
        state.Hash
            .Should()
            .Be(
                expectedHash
            );
        state.ScriptAssembly
            .Should()
            .Be(
                expectedScriptAssembly
            );
    }
}
