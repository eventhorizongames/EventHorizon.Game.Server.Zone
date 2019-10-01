using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.TestUtils;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Interpreters
{
    public class PrioritySelectorInterpreterTests
    {
        [Fact]
        public async Task ShouldRemoveSelfFromLastTraversalStackWhenContainedInLastTraversalStack()
        {
            // Given
            var expectedTraversal = BehaviorNodeStatus.VISITING.ToString();
            var expectedFirstChildNode = BehaviorNodeStatus.SUCCESS.ToString();
            var expectedRunningNode = BehaviorNodeStatus.RUNNING.ToString();
            var expectedLastNode = BehaviorNodeStatus.READY.ToString();
            var actor = new DefaultEntity();
            var state = new BehaviorTreeStateBuilder()
                .Root(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.RUNNING.ToString()
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.READY.ToString()
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.RUNNING.ToString()
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.READY.ToString()
                    }
                )
                .BuildWithLastTraversalStack()
                .PopActiveNodeFromQueue()
                .PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new PrioritySelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Collection(
                actual.NodeMap.Values,
                node => Assert.Equal(
                    expectedTraversal, 
                    node.Status
                ),
                node => Assert.Equal(
                    expectedFirstChildNode, 
                    node.Status
                ),
                node => Assert.Equal(
                    expectedRunningNode, 
                    node.Status
                )
            );
        }
        [Fact]
        public async Task ShouldReturnErrorStatusWhenNoChildrenAreInTraversal()
        {
            // Given
            var expectedTraversal = BehaviorNodeStatus.ERROR.ToString();
            var actor = new DefaultEntity();
            var state = new BehaviorTreeStateBuilder()
                .Root(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.VISITING.ToString()
                    }
                )
                .Build()
                .PopActiveNodeFromQueue()
                .PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new PrioritySelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Collection(
                actual.NodeMap.Values,
                node => Assert.Equal(
                    expectedTraversal,
                    node.Status
                )
            );
        }
        [Fact]
        public async Task ShouldReturnRunningStatusWhenChildrenAreRunning()
        {
            // Given
            var expectedStatus = BehaviorNodeStatus.RUNNING.ToString();
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
                        Status = BehaviorNodeStatus.RUNNING.ToString()
                    }
                ).Build()
                .PopActiveNodeFromQueue()
                .PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new PrioritySelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Collection(
                actual.NodeMap.Values,
                node => Assert.Equal(
                    expectedStatus,
                    node.Status
                ),
                node => Assert.Equal(
                    expectedStatus,
                    node.Status
                )
            );
            Assert.True(
                actual.CheckTraversal
            );
        }
        [Fact]
        public async Task ShouldRunNextNodeWhenChildHasReadyStatusAndNoSuccessOrRunning()
        {
            // Given
            var expectedStatus = BehaviorNodeStatus.VISITING.ToString();
            var expectedActiveNodeStatus = BehaviorNodeStatus.READY.ToString();
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
                .PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new PrioritySelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Collection(
                actual.NodeMap.Values,
                node => Assert.Equal(
                    expectedStatus,
                    node.Status
                ),
                node => Assert.Equal(
                    expectedActiveNodeStatus,
                    node.Status
                )
            );
            Assert.Equal(
                expectedActiveNodeStatus,
                actual.ActiveNode.Status
            );
            Assert.False(
                actual.CheckTraversal
            );
        }
        [Fact]
        public async Task ShouldReturnErrorStatusWhenChildrenAreAlsoError()
        {
            // Given
            var expectedTraversalStatus = BehaviorNodeStatus.ERROR.ToString();
            var expectedChildStatus = BehaviorNodeStatus.ERROR.ToString();
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
                        Status = BehaviorNodeStatus.ERROR.ToString()
                    }
                )
                .Build()
                .PopActiveNodeFromQueue()
                .PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new PrioritySelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Collection(
                actual.NodeMap.Values,
                node => Assert.Equal(
                    expectedTraversalStatus,
                    node.Status
                ),
                node => Assert.Equal(
                    expectedChildStatus,
                    node.Status
                )
            );
        }
        [Fact]
        public async Task ShouldKeepVisitingTraversalWhenChildIsReadyStatus()
        {
            // Given
            var expectedTraversalStatus = BehaviorNodeStatus.VISITING.ToString();
            var expectedChildStatus = BehaviorNodeStatus.READY.ToString();
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
                )
                .Build()
                .PopActiveNodeFromQueue()
                .PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new PrioritySelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Collection(
                actual.NodeMap.Values,
                node => Assert.Equal(
                    expectedTraversalStatus,
                    node.Status
                ),
                node => Assert.Equal(
                    expectedChildStatus,
                    node.Status
                )
            );
        }
        [Fact]
        public async Task ShouldKeepVisitingTraversalWhenChildIsFailedStatus()
        {
            // Given
            var expectedTraversalStatus = BehaviorNodeStatus.VISITING.ToString();
            var expectedChildStatus = BehaviorNodeStatus.FAILED.ToString();
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
                        Status = BehaviorNodeStatus.FAILED.ToString()
                    }
                )
                .Build()
                .PopActiveNodeFromQueue()
                .PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new PrioritySelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Collection(
                actual.NodeMap.Values,
                node => Assert.Equal(
                    expectedTraversalStatus,
                    node.Status
                ),
                node => Assert.Equal(
                    expectedChildStatus,
                    node.Status
                )
            );
        }
        [Fact]
        public async Task ShouldKeepVisitingTraversalWhenChildIsVisitingStatus()
        {
            // Given
            var expectedTraversalStatus = BehaviorNodeStatus.VISITING.ToString();
            var expectedChildStatus = BehaviorNodeStatus.VISITING.ToString();
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
                        Status = BehaviorNodeStatus.VISITING.ToString()
                    }
                )
                .Build()
                .PopActiveNodeFromQueue()
                .PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new PrioritySelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Collection(
                actual.NodeMap.Values,
                node => Assert.Equal(
                    expectedTraversalStatus,
                    node.Status
                ),
                node => Assert.Equal(
                    expectedChildStatus,
                    node.Status
                )
            );
        }
    }
}