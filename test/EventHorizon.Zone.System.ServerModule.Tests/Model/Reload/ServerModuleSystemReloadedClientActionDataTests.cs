namespace EventHorizon.Zone.System.ServerModule.Tests.Model.Reload
{
    using EventHorizon.Zone.System.ServerModule.Model;
    using EventHorizon.Zone.System.ServerModule.Model.Client;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using Xunit;

    public class ServerModuleSystemReloadedClientActionDataTests
    {
        [Fact]
        public void ShouldSetPassedInListToServerModuleScriptList()
        {
            // Given
            var expected = new List<ServerModuleScripts>();

            // When
            var actual = new ServerModuleSystemReloadedClientActionData(
                expected
            );

            // Then
            actual.ServerModuleScriptList
                .Should().BeEquivalentTo(expected);
        }
    }
}
