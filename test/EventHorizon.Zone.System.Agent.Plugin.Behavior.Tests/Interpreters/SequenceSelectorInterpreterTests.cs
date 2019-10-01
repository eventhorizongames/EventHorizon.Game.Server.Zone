using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.TestUtils;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Interpreters
{
    public class SequenceSelectorInterpreterTests
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
            var actionInterpreter = new SequenceSelectorInterpreter();
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
        public async Task ShouldSetStatusToFailedWhenChildIsNotSuccess()
        {
            // Given
            var expectedTraversal = BehaviorNodeStatus.FAILED.ToString();
            var expectedFirstChildNode = BehaviorNodeStatus.SUCCESS.ToString();
            var expectedSecondChildNode = BehaviorNodeStatus.SUCCESS.ToString();
            var expectedThirdChildNode = BehaviorNodeStatus.FAILED.ToString();
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
                        Status = BehaviorNodeStatus.SUCCESS.ToString()
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.SUCCESS.ToString()
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
            var actionInterpreter = new SequenceSelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Collection(
                actual.NodeMap.Values,
                node => Assert.Equal(
                    expectedTraversal, node.Status
                ),
                node => Assert.Equal(
                    expectedFirstChildNode, 
                    node.Status
                ),
                node => Assert.Equal(
                    expectedSecondChildNode, 
                    node.Status
                ),
                node => Assert.Equal(
                    expectedThirdChildNode, 
                    node.Status
                )
            );
        }
        [Fact]
        public async Task ShouldSetStatusToErrorWhenChildIsNotSuccess()
        {
            // Given
            var expectedTraversal = BehaviorNodeStatus.ERROR.ToString();
            var expectedFirstChildNode = BehaviorNodeStatus.SUCCESS.ToString();
            var expectedSecondChildNode = BehaviorNodeStatus.SUCCESS.ToString();
            var expectedThirdChildNode = BehaviorNodeStatus.ERROR.ToString();
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
                        Status = BehaviorNodeStatus.SUCCESS.ToString()
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.SUCCESS.ToString()
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
            var actionInterpreter = new SequenceSelectorInterpreter();
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
                    expectedSecondChildNode, 
                    node.Status
                ),
                node => Assert.Equal(
                    expectedThirdChildNode, 
                    node.Status
                )
            );
        }
        [Fact]
        public async Task ShouldSetStatusToRunningWhenChildIsNotSuccess()
        {
            // Given
            var expectedTraversal = BehaviorNodeStatus.RUNNING.ToString();
            var expectedFirstChildNode = BehaviorNodeStatus.SUCCESS.ToString();
            var expectedSecondChildNode = BehaviorNodeStatus.SUCCESS.ToString();
            var expectedThirdChildNode = BehaviorNodeStatus.RUNNING.ToString();
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
                        Status = BehaviorNodeStatus.SUCCESS.ToString()
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.SUCCESS.ToString()
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.RUNNING.ToString()
                    }
                )
                .Build()
                .PopActiveNodeFromQueue()
                .PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new SequenceSelectorInterpreter();
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
                    expectedSecondChildNode, 
                    node.Status
                ),
                node => Assert.Equal(
                    expectedThirdChildNode, 
                    node.Status
                )
            );
        }
    }
}