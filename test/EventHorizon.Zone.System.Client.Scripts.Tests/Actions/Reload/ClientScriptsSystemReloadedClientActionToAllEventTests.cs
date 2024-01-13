namespace EventHorizon.Zone.System.Client.Scripts.Tests.Actions.Reload;

using EventHorizon.Zone.System.Client.Scripts.Actions.Reload;
using EventHorizon.Zone.System.Client.Scripts.Model;
using EventHorizon.Zone.System.Client.Scripts.Model.Client;

using FluentAssertions;

using global::System.Collections.Generic;

using Xunit;

public class ClientScriptsSystemReloadedClientActionToAllEventTests
{
    [Fact]
    public void ShouldHaveExpectedActionWhenCreated()
    {
        // Given
        var expected = "CLIENT_SCRIPTS_SYSTEM_RELOADED_CLIENT_ACTION_EVENT";
        var expectedDetails = new List<ClientScript>();

        // When
        var actual = ClientScriptsSystemReloadedClientActionToAllEvent.Create(
            new ClientScriptsSystemReloadedClientActionData(
                expectedDetails
            )
        );

        // Then
        actual.Action.Should().Be(
            expected
        );
        actual.Data
            .As<ClientScriptsSystemReloadedClientActionData>()
            .ClientScriptList
            .Should()
            .BeEquivalentTo(
                expectedDetails
            );
    }
}
