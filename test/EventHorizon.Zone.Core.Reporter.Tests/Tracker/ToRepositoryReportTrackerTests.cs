using FluentAssertions;
using EventHorizon.Zone.Core.Reporter.Tracker;
using Xunit;

namespace EventHorizon.Zone.Core.Reporter.Tests.Tracker
{
    public class ToRepositoryReportTrackerTests
    {
        [Fact]
        public void TestShouldKeepRunningListOfTrackedReportsByIdWhenTrackingMessagesAndData()
        {
            // Given
            var expectedReportId = "report-id";
            var expectedReportItemMessage = "reported-message";
            var reportItemData = "report-data";
            var expectedReportItemData = @"""report-data""";

            // When
            var reportTracker = new ToRepositoryReportTracker();
            reportTracker.Track(
                expectedReportId,
                expectedReportItemMessage,
                reportItemData
            );
            var actualCollection = reportTracker.TakeAll();

            // Then
            actualCollection
                .Should()
                .NotBeEmpty()
                .And
                .SatisfyRespectively(
                    actual =>
                    {
                        actual.Id
                            .Should()
                            .Be(
                                expectedReportId
                            );

                        actual.ItemList
                            .Should()
                            .NotBeEmpty()
                            .And
                            .SatisfyRespectively(
                                reportItem =>
                                {
                                    reportItem.Message
                                        .Should()
                                        .Contain(
                                            expectedReportItemMessage
                                        );
                                    reportItem.Data
                                        .Should()
                                        .Be(
                                            expectedReportItemData
                                        );
                                }
                            );
                    }
                );
        }

        [Fact]
        public void TestShouldClearAndExistingReportsWhenTrackerIsCalledWithReportsId()
        {
            // Given
            var reportId = "report-id";

            // When
            var reportTracker = new ToRepositoryReportTracker();
            reportTracker.Track(
                reportId,
                "message",
                "data"
            );
            reportTracker
                .TakeAll()
                .Should()
                .NotBeEmpty();

            reportTracker.Clear(
                reportId
            );

            // Then
            reportTracker
                .TakeAll()
                .Should()
                .BeEmpty();
        }
    }
}