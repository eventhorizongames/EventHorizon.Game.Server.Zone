namespace EventHorizon.Zone.Core.Reporter.Tests.Writer
{
    using System;
    using System.Dynamic;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Reporter.Model;
    using EventHorizon.Zone.Core.Reporter.Writer;
    using EventHorizon.Zone.Core.Reporter.Writer.Client;

    using FluentAssertions;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class WriteReportToElasticsearchHandlerTests
    {
        [Fact]
        public async Task ShouldNotCallBlukAsyncWhenIsNotConnected()
        {
            // Given

            var loggerMock = new Mock<ILogger<WriteReportToElasticsearchHandler>>();
            var clientMock = new Mock<ElasticsearchReporterClient>();


            // When
            var handler = new WriteReportToElasticsearchHandler(
                loggerMock.Object,
                clientMock.Object
            );
            await handler.Handle(
                new WriteReportToElasticsearch(
                    new Report("report-id", DateTime.UtcNow)
                ),
                CancellationToken.None
            );

            // Then
            clientMock.Verify(
                mock => mock.BulkAsync(
                    It.IsAny<object[]>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldCallBulkAsyncWithEmptyIndexesWhenNoItemsAreInReportList()
        {
            // Given
            var reportId = "report-id";
            var timestamp = DateTime.UtcNow;
            var report = new Report(
                reportId,
                timestamp
            );

            var loggerMock = new Mock<ILogger<WriteReportToElasticsearchHandler>>();
            var clientMock = new Mock<ElasticsearchReporterClient>();

            clientMock.Setup(
                mock => mock.IsConnected
            ).Returns(
                true
            );


            // When
            var handler = new WriteReportToElasticsearchHandler(
                loggerMock.Object,
                clientMock.Object
            );
            await handler.Handle(
                new WriteReportToElasticsearch(
                    report
                ),
                CancellationToken.None
            );

            // Then
            clientMock.Verify(
                mock => mock.BulkAsync(
                    It.IsAny<object[]>(),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldBulkAsyncTheExpectedListWhenItemListOnReportContainsItems()
        {
            // Given

            var reportId = "report-id";
            var timestamp = DateTime.UtcNow;
            var report = new Report(
                reportId,
                timestamp
            );
            var reportItemCorrelationId = "report-item-1-correlation-id";
            var reportItemMessage = "report-item-1-message";
            var reportItemTimestamp = DateTime.UtcNow;
            var reportItemData = new { };
            var reportItem = new ReportItem(
                reportItemCorrelationId,
                reportItemMessage,
                reportItemTimestamp,
                reportItemData
            );
            report.AddItem(
                reportItem
            );
            var expectedIndexIndex = "report";
            var expectedIndexType = "_doc";
            var expectedIndexReport = new ReportIndexItem(
                report,
                reportItem
            );

            var loggerMock = new Mock<ILogger<WriteReportToElasticsearchHandler>>();
            var clientMock = new Mock<ElasticsearchReporterClient>();

            clientMock.Setup(
                mock => mock.IsConnected
            ).Returns(
                true
            );


            // When
            var handler = new WriteReportToElasticsearchHandler(
                loggerMock.Object,
                clientMock.Object
            );
            var actual = default(object[]);
            clientMock.Setup(
                mock => mock.BulkAsync(
                    It.IsAny<object[]>(),
                    CancellationToken.None
                )
            ).Callback<object[], CancellationToken>(
                (indexes, _) =>
                {
                    actual = indexes;
                }
            );
            await handler.Handle(
                new WriteReportToElasticsearch(
                    report
                ),
                CancellationToken.None
            );

            // Then
            actual.Length
                .Should().Be(2);
            actual[1]
                .Should().BeEquivalentTo(expectedIndexReport);

            VerifyReportIndex(
                actual[0],
                expectedIndexIndex,
                expectedIndexType
            );
        }

        private void VerifyReportIndex(
            object actualRawObject,
            string expectedIndexIndex,
            string expectedIndexType
        )
        {
            var actual = JsonSerializer.Deserialize<ReportIndexer>(
                JsonSerializer.Serialize(
                    actualRawObject
                )
            );

            actual.index._index
                .Should().Be(expectedIndexIndex);
            actual.index._type
                .Should().Be(expectedIndexType);
            actual.index._id
                .Should().NotBeEmpty();
        }

        public class ReportIndexer
        {
            public ReportIndexerIndex index { get; set; }
        }

        public class ReportIndexerIndex
        {
            public string _index { get; set; }
            public string _type { get; set; }
            public string _id { get; set; }
        }
    }
}
