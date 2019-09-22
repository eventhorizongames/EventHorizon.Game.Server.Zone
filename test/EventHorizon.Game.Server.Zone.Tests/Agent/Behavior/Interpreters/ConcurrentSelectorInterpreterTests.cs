using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.TestUtils;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.Interpreters
{
    public class ConcurrentSelectorInterpreterTests
    {

        [Fact]
        public async Task ShouldSetStatusOnTraversalNodeWhenFailGateIsTriggered()
        {
            // Given
            var expected = BehaviorNodeStatus.FAILED.ToString();
            var actor = new DefaultEntity();
            var state = new BehaviorTreeStateBuilder()
                .Root(
                    new SerializedBehaviorNode
                    {
                        FailGate = 1,
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.FAILED.ToString()
                    }
                )
                .Build()
                .PopActiveNodeFromQueue()
                .SetStatusOnActiveNode(
                    BehaviorNodeStatus.VISITING
                ).PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new ConcurrentSelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Collection(
                actual.NodeMap.Values,
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
        public async Task ShouldNotFailTraversalWhenFailGateIsNotTriggered()
        {
            // Given
            var expectedTraversal = BehaviorNodeStatus.VISITING.ToString();
            var expectedReadNode = BehaviorNodeStatus.READY.ToString();
            var expectedFailedNode = BehaviorNodeStatus.FAILED.ToString();
            var actor = new DefaultEntity();
            var state = new BehaviorTreeStateBuilder()
                .Root(
                    new SerializedBehaviorNode
                    {
                        FailGate = 2,
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.READY.ToString()
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.FAILED.ToString()
                    }
                )
                .Build()
                .PopActiveNodeFromQueue()
                .SetStatusOnActiveNode(
                    BehaviorNodeStatus.VISITING
                ).PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new ConcurrentSelectorInterpreter();
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
                    expectedReadNode, 
                    node.Status
                ),
                node => Assert.Equal(
                    expectedFailedNode, 
                    node.Status
                )
            );
        }
        [Fact]
        public async Task ShouldPopActiveTraversalNodeWhenFailGateIsTriggered()
        {
            // Given
            var expected = default(BehaviorNode);
            var actor = new DefaultEntity();
            var state = new BehaviorTreeStateBuilder()
                .Root(
                    new SerializedBehaviorNode
                    {
                        FailGate = 1,
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.FAILED.ToString()
                    }
                )
                .Build()
                .PopActiveNodeFromQueue()
                .SetStatusOnActiveNode(
                    BehaviorNodeStatus.VISITING
                ).PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new ConcurrentSelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Equal(
                expected,
                actual.ActiveTraversal
            );
        }
        [Fact]
        public async Task ShouldAdvanceTheQueueToAfterLastChildOfTraversalNodeWhenFailGateIsTriggered()
        {
            // Given
            var expected = default(BehaviorNode);
            var actor = new DefaultEntity();
            var state = new BehaviorTreeStateBuilder()
                .Root(
                    new SerializedBehaviorNode
                    {
                        FailGate = 1,
                    }
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.FAILED.ToString()
                    }
                )
                .Build()
                .PopActiveNodeFromQueue()
                .SetStatusOnActiveNode(
                    BehaviorNodeStatus.VISITING
                ).PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new ConcurrentSelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Equal(
                expected,
                actual.ActiveNode
            );
        }
        [Fact]
        public async Task ShouldHaveStateOfRunningWhenAnyChildrenAreRunning()
        {
            // Given
            var expected = BehaviorNodeStatus.RUNNING.ToString();
            var actor = new DefaultEntity();
            var state = new BehaviorTreeStateBuilder()
                .Root(
                    new SerializedBehaviorNode()
                ).AddNode(
                    new SerializedBehaviorNode
                    {
                        Status = BehaviorNodeStatus.RUNNING.ToString()
                    }
                )
                .Build()
                .PopActiveNodeFromQueue()
                .SetStatusOnActiveNode(
                    BehaviorNodeStatus.VISITING
                ).PushActiveNodeToTraversalStack();

            // When
            var actionInterpreter = new ConcurrentSelectorInterpreter();
            var actual = await actionInterpreter.Run(
                actor,
                state
            );

            // Then
            Assert.Collection(
                actual.NodeMap.Values,
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
    }
}