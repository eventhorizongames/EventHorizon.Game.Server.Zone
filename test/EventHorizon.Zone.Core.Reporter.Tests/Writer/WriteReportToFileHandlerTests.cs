namespace EventHorizon.Zone.Core.Reporter.Tests.Writer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Reporter.Model;
    using EventHorizon.Zone.Core.Reporter.Writer;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class WriteReportToFileHandlerTests
    {
        [Fact]
        public async Task TestShouldNotSaveReportsWhenReportingDirectoryIsNotCreated()
        {
            // Given
            var appDataPath = "app-data-path";
            var reportingPath = Path.Combine(
                appDataPath,
                "Reporting"
            );

            var loggerMock = new Mock<ILogger<WriteReportToFileHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new WriteReportToFileHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            await handler.Handle(
                new WriteReportToFile(
                    new Report()
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<AppendTextToFile>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task TestShouldNotWriteTextToFileWhenNoReportsAreReturned()
        {
            // Given
            var appDataPath = "app-data-path";
            var reportingPath = Path.Combine(
                appDataPath,
                "Reporting"
            );
            var emptyReportList = new List<Report>();

            var loggerMock = new Mock<ILogger<WriteReportToFileHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var reportRepositoryMock = new Mock<ReportRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<CreateDirectory>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            reportRepositoryMock.Setup(
                mock => mock.TakeAll()
            ).Returns(
                emptyReportList
            );

            // When
            var handler = new WriteReportToFileHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            await handler.Handle(
                new WriteReportToFile(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<AppendTextToFile>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task TestShouldWriteTextToFileWhenReportsAreReturned()
        {
            // Given
            var appDataPath = "app-data-path";
            var reportingPath = Path.Combine(
                appDataPath,
                "Reporting"
            );

            var id = "report-1-Id";
            var timestamp = DateTime.Now;
            var message = "report-1-Message";
            var correlationId = "report-1-CorrelationId";
            var data = "report-1-Data";
            var expectedReport1FileFullName = Path.Combine(
                reportingPath,
                "Reporting_report-1-Id.log"
            );
            var expectedMessage = $"\"{data}\"";

            var expectedReport1Text = string.Join(
                "",
                new string[] {
                    "---",
                    Environment.NewLine,
                    message,
                    Environment.NewLine,
                    expectedMessage,
                    Environment.NewLine,
                    "---",
                    Environment.NewLine,
                    Environment.NewLine,
                    Environment.NewLine,
                }
            );

            var report = new Report(
                id,
                timestamp
            ).AddItem(
                new ReportItem(
                    correlationId,
                    message,
                    timestamp,
                    data
                )
            );
            var reportList = new List<Report>()
            {
                report,
            };

            var loggerMock = new Mock<ILogger<WriteReportToFileHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<CreateDirectory>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new WriteReportToFileHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            await handler.Handle(
                new WriteReportToFile(
                    report
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new AppendTextToFile(
                        expectedReport1FileFullName,
                        expectedReport1Text
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}
