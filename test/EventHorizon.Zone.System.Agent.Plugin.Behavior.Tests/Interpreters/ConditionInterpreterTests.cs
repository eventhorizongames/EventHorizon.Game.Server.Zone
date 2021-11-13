namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Interpreters
{
    using EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.TestUtils;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script.Run;

    using global::System;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class ConditionInterpreterTests
    {
        [Fact]
        public async Task ShouldSetStatusToSuccessWhenActiveNodeIsRunning()
        {
            // Given
            var expected = BehaviorNodeStatus.SUCCESS.ToString();
            var actor = new DefaultEntity();
            var state = new BehaviorTreeStateBuilder()
                .Root(new SerializedBehaviorNode
                {
                    Status = BehaviorNodeStatus.RUNNING.ToString()
                })
                .Build()
                .PopActiveNodeFromQueue();

            var loggerMock = new Mock<ILogger<ConditionInterpreter>>();
            var mediatorMock = new Mock<IMediator>();
            var serviceScopeFactoryMock = ServiceScopeFactoryUtils
                .SetupServiceScopeFactoryWithMediatorMock(
                    mediatorMock
                );

            mediatorMock.Setup(
                mediator => mediator.Send(
                    It.IsAny<RunBehaviorScript>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new BehaviorScriptResponse(
                    BehaviorNodeStatus.SUCCESS
                )
            );

            // When
            var conditionInterpreter = new ConditionInterpreter(
                loggerMock.Object,
                serviceScopeFactoryMock.Object
            );
            var actual = await conditionInterpreter.Run(
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
        public async Task ShouldSetTravesalToCheckWhenActiveNodeIsRunning()
        {
            // Given
            var actor = new DefaultEntity();
            var state = new BehaviorTreeStateBuilder()
                .Root(new SerializedBehaviorNode
                {
                    Status = BehaviorNodeStatus.VISITING.ToString()
                }).AddNode(new SerializedBehaviorNode
                {
                    Status = BehaviorNodeStatus.RUNNING.ToString()
                }).Build()
                .PopActiveNodeFromQueue()
                .PushActiveNodeToTraversalStack()
                .PopActiveNodeFromQueue();

            var loggerMock = new Mock<ILogger<ConditionInterpreter>>();
            var mediatorMock = new Mock<IMediator>();
            var serviceScopeFactoryMock = ServiceScopeFactoryUtils
                .SetupServiceScopeFactoryWithMediatorMock(
                    mediatorMock
                );

            mediatorMock.Setup(
                mediator => mediator.Send(
                    It.IsAny<RunBehaviorScript>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new BehaviorScriptResponse(
                    BehaviorNodeStatus.SUCCESS
                )
            );

            // When
            var conditionInterpreter = new ConditionInterpreter(
                loggerMock.Object,
                serviceScopeFactoryMock.Object
            );
            var actual = await conditionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.True(
                actual.CheckTraversal
            );
        }
        [Fact]
        public async Task ShouldSetActiveTraversalAndActiveConditionNodeToFailedWhenNotSuccessOrRunningFromRunOfScript()
        {
            // Given
            var expected = BehaviorNodeStatus.FAILED.ToString();
            var actor = new DefaultEntity();
            var state = new BehaviorTreeStateBuilder()
                .Root(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.VISITING.ToString()
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.READY.ToString()
                    }
                ).Build()
                .PopActiveNodeFromQueue()
                .PushActiveNodeToTraversalStack()
                .PopActiveNodeFromQueue();

            var loggerMock = new Mock<ILogger<ConditionInterpreter>>();
            var mediatorMock = new Mock<IMediator>();
            var serviceScopeFactoryMock = ServiceScopeFactoryUtils
                .SetupServiceScopeFactoryWithMediatorMock(
                    mediatorMock
                );

            mediatorMock.Setup(
                mediator => mediator.Send(
                    It.IsAny<RunBehaviorScript>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new BehaviorScriptResponse(
                    BehaviorNodeStatus.FAILED
                )
            );

            // When
            var conditionInterpreter = new ConditionInterpreter(
                loggerMock.Object,
                serviceScopeFactoryMock.Object
            );
            var actual = await conditionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Collection(
                actual.NodeList(),
                node => Assert.Equal(
                    expected,
                    node.Status
                ),
                node => Assert.Equal(
                    expected,
                    node.Status
                )
            );
            Assert.True(
                actual.CheckTraversal
            );
        }
        [Fact]
        public async Task ShouldConditionToFailedWhenAnyExceptionIsThrownFromRunOfScript()
        {
            // Given
            var expected = BehaviorNodeStatus.FAILED.ToString();
            var actor = new DefaultEntity();
            var state = new BehaviorTreeStateBuilder()
                .Root(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.VISITING.ToString()
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.READY.ToString()
                    }
                ).Build()
                .PopActiveNodeFromQueue()
                .PushActiveNodeToTraversalStack()
                .PopActiveNodeFromQueue();

            var loggerMock = new Mock<ILogger<ConditionInterpreter>>();
            var mediatorMock = new Mock<IMediator>();
            var serviceScopeFactoryMock = ServiceScopeFactoryUtils
                .SetupServiceScopeFactoryWithMediatorMock(
                    mediatorMock
                );

            mediatorMock.Setup(
                mediator => mediator.Send(
                    It.IsAny<RunBehaviorScript>(),
                    CancellationToken.None
                )
            ).ThrowsAsync(
                new Exception(
                    "Script Error"
                )
            );

            // When
            var conditionInterpreter = new ConditionInterpreter(
                loggerMock.Object,
                serviceScopeFactoryMock.Object
            );
            var actual = await conditionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Collection(
                actual.NodeList(),
                node => Assert.Equal(
                    expected,
                    node.Status
                ),
                node => Assert.Equal(
                    expected,
                    node.Status
                )
            );
        }

        [Fact]
        public async Task ShouldSetTravesalToCheckWhenActiveNodeIsNotRunningOrReady()
        {
            // Given
            var actor = new DefaultEntity();
            var state = StandardBehaviorTreeState
                .CreateNodeWithTraversal()
                .PopActiveNodeFromQueue()
                .PushActiveNodeToTraversalStack()
                .PopActiveNodeFromQueue()
                .SetStatusOnActiveNode(
                    BehaviorNodeStatus.FAILED
                );

            var loggerMock = new Mock<ILogger<ConditionInterpreter>>();
            var mediatorMock = new Mock<IMediator>();
            var serviceScopeFactoryMock = ServiceScopeFactoryUtils
                .SetupServiceScopeFactoryWithMediatorMock(
                    mediatorMock
                );

            // When
            var conditionInterpreter = new ConditionInterpreter(
                loggerMock.Object,
                serviceScopeFactoryMock.Object
            );
            var actual = await conditionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.True(
                actual.CheckTraversal
            );
        }
    }
}
