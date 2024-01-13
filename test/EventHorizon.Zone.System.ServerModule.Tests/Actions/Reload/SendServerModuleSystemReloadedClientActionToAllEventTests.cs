namespace EventHorizon.Zone.System.ServerModule.Tests.Actions.Reload;

using EventHorizon.Zone.System.ServerModule.Actions.Reload;
using EventHorizon.Zone.System.ServerModule.Model;
using EventHorizon.Zone.System.ServerModule.Model.Client;

using FluentAssertions;

using global::System.Collections.Generic;

using Xunit;

public class SendServerModuleSystemReloadedClientActionToAllEventTests
{
    [Fact]
    public void ShouldCreateGenericClientActionToAll()
    {
        // Given
        var expectedAction = "SERVER_MODULE_SYSTEM_RELOADED_CLIENT_ACTION_EVENT";
        var expectedData = new ServerModuleSystemReloadedClientActionData(
            new List<ServerModuleScripts>()
        );

        // When
        var actual = SendServerModuleSystemReloadedClientActionToAllEvent.Create(
            expectedData
        );

        // Then
        actual.Action
            .Should().Be(expectedAction);
        actual.Data
            .Should().Be(expectedData);
    }
}
