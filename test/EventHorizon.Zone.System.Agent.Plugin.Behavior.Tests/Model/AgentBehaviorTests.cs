namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Model;

using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

using global::System;

using Xunit;

public class AgentBehaviorTests
{
    [Fact]
    public void TestShouldBeAbleToAccessPropertiesThroughIndexBasedLookup(
    )
    {
        var expectedIsEnabled = true;
        var isEnabledValue = true;
        var isEnabledIndex = "isEnabled";

        var expectedTreeId = "tree-id";
        var treeIdValue = "tree-id";
        var treeIdIndex = "treeId";

        var expectedNextTickRequest = DateTime.MinValue;
        var nextTickRequestValue = DateTime.MinValue;
        var nextTickRequestIndex = "nextTickRequest";

        // When
        var actual = new AgentBehavior();
        actual[isEnabledIndex] = isEnabledValue;
        actual[treeIdIndex] = treeIdValue;
        actual[nextTickRequestIndex] = nextTickRequestValue;

        // Then
        Assert.Equal(
            expectedIsEnabled,
            actual[isEnabledIndex]
        );
        Assert.Equal(
            expectedTreeId,
            actual[treeIdIndex]
        );
        Assert.Equal(
            expectedNextTickRequest,
            actual[nextTickRequestIndex]
        );
    }

    [Fact]
    public void TestShouldReturnNullWhenIndexIsNotSupported()
    {
        // Given
        var invalidIndex = "invalid-index";

        // When
        var agentBehavior = new AgentBehavior();
        agentBehavior[invalidIndex] = "something-invalid";
        var actual = agentBehavior[invalidIndex];

        // Then
        Assert.Null(
            actual
        );
    }
}
