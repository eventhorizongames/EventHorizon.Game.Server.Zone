namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Interpreters
{
    using EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.TestUtils;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

    using global::System.Collections.Generic;
    using global::System.Threading.Tasks;

    using Xunit;

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
                actual.NodeList(),
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
                actual.NodeList(),
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
                actual.NodeList(),
                node => Assert.Equal(
                    expectedStatus,
                    node.Status
                ),
                node => Assert.Equal(
                    expectedStatus,
                    node.Status
                )
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
                actual.NodeList(),
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
                actual.NodeList(),
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
                actual.NodeList(),
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
        public async Task ShouldErrorTraversalWhenAllChildenAreFailedStatus()
        {
            // Given
            var expectedTraversalStatus = BehaviorNodeStatus.ERROR.ToString();
            var expectedChildStatus = BehaviorNodeStatus.FAILED.ToString();
            var actor = new DefaultEntity();
            var childNode = new SerializedBehaviorNode
            {
                Status = BehaviorNodeStatus.FAILED.ToString()
            };
            var state = new BehaviorTreeStateBuilder()
                .Root(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.VISITING.ToString(),
                        NodeList = new List<SerializedBehaviorNode>
                        {
                            childNode
                        }
                    }
                ).AddNode(
                    childNode
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
                actual.NodeList(),
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
                actual.NodeList(),
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
        public async Task TestShouldReturnVisitingActiveTraversalStatusWhenTransversalIsSetToResetAndContainedInLastTraversal()
        {
            // Given
            var expected = BehaviorNodeStatus.VISITING.ToString();
            var actor = new DefaultEntity();
            var currentTraversalNode = new SerializedBehaviorNode
            {
                Reset = true,
                Status = BehaviorNodeStatus.READY.ToString()
            };
            var shape = new ActorBehaviorTreeShape(
                "shape",
                new SerializedAgentBehaviorTree
                {
                    Root = currentTraversalNode
                }
            );
            var state = new BehaviorTreeStateBuilder()
                .Root(
                    currentTraversalNode
                )
                .Build()
                .SetShape(
                    shape
                ).PopActiveNodeFromQueue()
                .PushActiveNodeToTraversalStack()
                .AddActiveTraversalToNextStack()
                .SetShape(
                    shape
                ).PopActiveTraversalNode();

            // When
            var actionInterpreter = new PrioritySelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Equal(
                expected,
                actual.ActiveTraversal.Status
            );
            Assert.Collection(
                actual.NodeList(),
                node => Assert.Equal(
                    expected,
                    node.Status
                )
            );
        }
    }
}
