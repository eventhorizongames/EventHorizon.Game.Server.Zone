namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Interpreters;

using EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.TestUtils;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

using global::System;
using global::System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class ActionInterpreterTests
{
    [Fact]
    public async Task ShouldSetStateStatusForActiveNodeToFailedOnAnyError()
    {
        // Given
        var expected = BehaviorNodeStatus.FAILED.ToString();
        var actor = new DefaultEntity();
        var state = StandardBehaviorTreeState
            .CreateSingleNode()
            .PopActiveNodeFromQueue();

        var loggerMock = new Mock<ILogger<ActionInterpreter>>();
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

        serviceScopeFactoryMock.Setup(
            a => a.CreateScope()
        ).Throws(
            new Exception(
                "An Error"
            )
        );

        // When
        var actionInterpreter = new ActionInterpreter(
            loggerMock.Object,
            serviceScopeFactoryMock.Object
        );
        var actual = await actionInterpreter.Run(
            actor,
            state
        );

        // Then
        Assert.Equal(
            expected,
            actual.ActiveNode.Status
        );
    }

    [Fact]
    public async Task ShouldSetTravesalToCheckForActiveNodeToFailedOnAnyError()
    {
        // Given
        var expected = BehaviorNodeStatus.FAILED.ToString();
        var actor = new DefaultEntity();
        var state = StandardBehaviorTreeState
            .CreateNodeWithTraversal()
            .PopActiveNodeFromQueue()
            .PushActiveNodeToTraversalStack()
            .PopActiveNodeFromQueue();

        var loggerMock = new Mock<ILogger<ActionInterpreter>>();
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

        serviceScopeFactoryMock.Setup(
            a => a.CreateScope()
        ).Throws(
            new Exception(
                "An Error"
            )
        );

        // When
        var actionInterpreter = new ActionInterpreter(
            loggerMock.Object,
            serviceScopeFactoryMock.Object
        );
        var actual = await actionInterpreter.Run(
            actor,
            state
        );

        // Then
        Assert.True(
            actual.CheckTraversal
        );
    }

    [Fact]
    public async Task ShouldSetTravesalToCheckWhenActiveNodeNotReadyOrRunning()
    {
        // Given
        var expected = BehaviorNodeStatus.FAILED.ToString();
        var actor = new DefaultEntity();
        var state = StandardBehaviorTreeState
            .CreateNodeWithTraversal()
            .PopActiveNodeFromQueue()
            .PushActiveNodeToTraversalStack()
            .PopActiveNodeFromQueue()
            .SetStatusOnActiveNode(
                BehaviorNodeStatus.FAILED
            );

        var loggerMock = new Mock<ILogger<ActionInterpreter>>();
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

        serviceScopeFactoryMock.Setup(
            a => a.CreateScope()
        ).Throws(
            new Exception(
                "An Error"
            )
        );

        // When
        var actionInterpreter = new ActionInterpreter(
            loggerMock.Object,
            serviceScopeFactoryMock.Object
        );
        var actual = await actionInterpreter.Run(
            actor,
            state
        );

        // Then
        Assert.True(
            actual.CheckTraversal
        );
    }
}
