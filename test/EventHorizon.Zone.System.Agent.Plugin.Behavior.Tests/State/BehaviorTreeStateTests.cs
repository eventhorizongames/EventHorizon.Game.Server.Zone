using System.Collections.Generic;
using EventHorizon.Zone.Core.Reporter.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.State
{
    public class BehaviorTreeStateTests
    {
        [Fact]
        public void TestShouldBeInvalidNodeWhenActiveNodeTokenIsZero()
        {
            // Given
            var treeShape = new ActorBehaviorTreeShape(
                new SerializedAgentBehaviorTree
                {
                    Root = new SerializedBehaviorNode()
                }
            );
            // When
            var behaviorTreeState = new BehaviorTreeState(
                treeShape
            );
            var actual = behaviorTreeState.IsActiveNodeValidAndNotRunning();

            // Then
            Assert.False(
                actual
            );
        }

        [Fact]
        public void TestShouldReturnLastChildTokenIdWhenActiveTraversalHasChildrent()
        {
            // Given
            var expectedTraversalChild = new BehaviorNode();
            var expected = expectedTraversalChild.Token;
            var activeTraversalNode = new BehaviorNode(
                new SerializedBehaviorNode()
            );
            activeTraversalNode.NodeList.Add(
                expectedTraversalChild
            );

            // When
            var state = new BehaviorTreeState(
                new ActorBehaviorTreeShape
                {
                    NodeList = new List<BehaviorNode>
                    {
                        activeTraversalNode
                    }
                }
            ).PopActiveNodeFromQueue()
            .PushActiveNodeToTraversalStack();

            var actual = state.GetActiveTraversalLastChild();

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void TestShouldUseSetReportTrackerWhenReportAndClearAreCalled()
        {
            // Given
            var expectedReportId = "report-id";
            var expectedReportMessage = "report-message";
            var expectedReportData = "report-data";

            var reportTrackerMock = new Mock<ReportTracker>();

            // When
            var state = new BehaviorTreeState();
            state.SetReportTracker(
                expectedReportId,
                reportTrackerMock.Object
            );
            state.Report(
                expectedReportMessage,
                expectedReportData
            );
            state.ClearReport();
            
            // Then
            reportTrackerMock.Verify(
                mock => mock.Track(
                    expectedReportId,
                    expectedReportMessage,
                    expectedReportData
                )
            );
            reportTrackerMock.Verify(
                mock => mock.Clear(
                    expectedReportId
                )
            );
        }

        [Fact]
        public void TestShouldSendEmptyStringWhenNullOrNoDataIsReported()
        {
            // Given
            var expectedReportId = "report-id";
            var expectedReportMessage = "report-message";
            var expectedReportData = "";

            var reportTrackerMock = new Mock<ReportTracker>();

            // When
            var state = new BehaviorTreeState();
            state.SetReportTracker(
                expectedReportId,
                reportTrackerMock.Object
            );
            state.Report(
                expectedReportMessage,
                expectedReportData
            );
            state.ClearReport();
            
            // Then
            reportTrackerMock.Verify(
                mock => mock.Track(
                    expectedReportId,
                    expectedReportMessage,
                    expectedReportData
                )
            );
            reportTrackerMock.Verify(
                mock => mock.Clear(
                    expectedReportId
                )
            );
        }

        [Fact]
        public void TestShouldNotFailReportOrClearReportWhenReportTrackerIsNull()
        {
            // Given
            var reportId = "report-id";
            var reportMessage = "report-message";
            var reportData = "report-data";

            ReportTracker reportTracker = null;

            // When
            var state = new BehaviorTreeState();
            state.SetReportTracker(
                reportId,
                reportTracker
            );
            state.Report(
                reportMessage,
                reportData
            );
            state.ClearReport();
            
            // Then
            Assert.True(
                true, 
                "This will always be successful, if not it will throw an expection."
            );
        }
    }
}