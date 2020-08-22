namespace EventHorizon.Zone.Core.Reporter.Tests.Tracker
{
    using FluentAssertions;
    using EventHorizon.Zone.Core.Reporter.Tracker;
    using Xunit;
    using Moq;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using System;
    using EventHorizon.Zone.Core.Reporter.Model;

    public class ReportTrackingRepositoryTests
    {
        [Fact]
        public void TestShouldKeepRunningListOfTrackedReportsByIdWhenTrackingMessagesAndData()
        {
            // Given
            var now = DateTime.Now;
            var reportId = "report-id";
            var correlationId = "correlation-id";
            var message = "reported-message";
            var reportItemData = "report-data";

            var expectedReportId = reportId;
            var expectedTimestamp = now;
            var expectedReportItemMessage = message;
            var expectedReportItemData = "report-data";

            var dateTimeMock = new Mock<IDateTimeService>();
            var reporterSettingsMock = new Mock<ReporterSettings>();

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var reportTracker = new ReportTrackingRepository(
                dateTimeMock.Object
            );
            reportTracker.Track(
                reportId,
                correlationId,
                message,
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
                            .Be(expectedReportId);
                        actual.Timestamp
                            .Should().Be(expectedTimestamp);

                        actual.ItemList
                            .Should()
                            .NotBeEmpty()
                            .And
                            .SatisfyRespectively(
                                reportItem =>
                                {
                                    reportItem.Message
                                        .Should()
                                        .Be(expectedReportItemMessage);

                                    reportItem.Data
                                        .Should()
                                        .Be(expectedReportItemData);
                                }
                            );
                    }
                );
        }

        [Fact]
        public void ShouldAddReportItemToExistingReportWhenReportAlreadyExists()
        {
            // Given
            var now = DateTime.Now;
            var reportId = "report-id";
            var correlationId = "correlation-id";
            var message = "reported-message";
            var reportItem1Data = "report-data-1";
            var reportItem2Data = "report-data-2";

            var expectedReportId = reportId;
            var expectedTimestamp = now;
            var expectedReportItemMessage = message;
            var expectedReportItem1Data = "report-data-1";
            var expectedReportItem2Data = "report-data-2";

            var dateTimeMock = new Mock<IDateTimeService>();

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var reportTracker = new ReportTrackingRepository(
                dateTimeMock.Object
            );
            reportTracker.Track(
                reportId,
                correlationId,
                message,
                reportItem1Data
            );
            reportTracker.Track(
                reportId,
                correlationId,
                message,
                reportItem2Data
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
                            .Be(expectedReportId);
                        actual.Timestamp
                            .Should().Be(expectedTimestamp);

                        actual.ItemList
                            .Should()
                            .NotBeEmpty()
                            .And
                            .SatisfyRespectively(
                                reportItem =>
                                {
                                    reportItem.Message
                                        .Should()
                                        .Be(expectedReportItemMessage);

                                    reportItem.Data
                                        .Should()
                                        .Be(expectedReportItem1Data);
                                },
                                reportItem =>
                                {
                                    reportItem.Message
                                        .Should()
                                        .Be(expectedReportItemMessage);

                                    reportItem.Data
                                        .Should()
                                        .Be(expectedReportItem2Data);
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
            var correlationId = "correlation-id";

            var dateTimeMock = new Mock<IDateTimeService>();

            // When
            var reportTracker = new ReportTrackingRepository(
                dateTimeMock.Object
            );
            reportTracker.Track(
                reportId,
                correlationId,
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