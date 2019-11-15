using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Reporter.Model;
using EventHorizon.Zone.Core.Reporter.Save;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventHorizon.Zone.Core.Reporter.Tests.Save
{
    public class SavePendingReportItemsEventHandlerTests
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

            var loggerMock = new Mock<ILogger<SavePendingReportItemsEventHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var reportRepositoryMock = new Mock<ReportRepository>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var handler = new SavePendingReportItemsEventHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                reportRepositoryMock.Object
            );
            await handler.Handle(
                new SavePendingReportItemsEvent(),
                CancellationToken.None
            );

            // Then
            reportRepositoryMock.Verify(
                mock => mock.TakeAll(),
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

            var loggerMock = new Mock<ILogger<SavePendingReportItemsEventHandler>>();
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
            var handler = new SavePendingReportItemsEventHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                reportRepositoryMock.Object
            );
            await handler.Handle(
                new SavePendingReportItemsEvent(),
                CancellationToken.None
            );

            // Then
            reportRepositoryMock.Verify(
                mock => mock.TakeAll()
            );
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

            var report1Id = "report-1-Id";
            var report1Message = "report-1-Message";
            var report1Data = "report-1-Data";
            var report2Id = "report-2-Id";

            var expectedReport1FileFullName = Path.Combine(
                reportingPath,
                "Reporting_report-1-Id.log"
            );
            var expectedReport1Text = String.Join(
                "",
                new string[] {
                    "---",
                    Environment.NewLine,
                    report1Message,
                    Environment.NewLine,
                    report1Data,
                    Environment.NewLine,
                    "---",
                    Environment.NewLine,
                    Environment.NewLine,
                    Environment.NewLine,
                }
            ); 
            var expectedReport2FileFullName = Path.Combine(
                reportingPath,
                "Reporting_report-2-Id.log"
            );
            var expectedReport2Text = "";
            var reportList = new List<Report>()
            {
                new Report(
                    report1Id
                ).AddItem(
                    new ReportItem(
                        report1Message,
                        report1Data
                    )
                ),
                new Report(
                    report2Id
                ),
            };

            var loggerMock = new Mock<ILogger<SavePendingReportItemsEventHandler>>();
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
                reportList
            );

            // When
            var handler = new SavePendingReportItemsEventHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                reportRepositoryMock.Object
            );
            await handler.Handle(
                new SavePendingReportItemsEvent(),
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
            mediatorMock.Verify(
                mock => mock.Send(
                    new AppendTextToFile(
                        expectedReport2FileFullName,
                        expectedReport2Text
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}