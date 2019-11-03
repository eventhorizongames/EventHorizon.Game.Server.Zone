using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Reporter.Model;
using EventHorizon.Zone.Core.Reporter.Save;
using FluentAssertions;
using Moq;
using Xunit;

namespace EventHorizon.Zone.Core.Reporter.Tests.Save
{
    public class SavePendingReportItemsEventHandlerTests
    {
        string savePath;

        public SavePendingReportItemsEventHandlerTests()
        {
            savePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Save",
                "ReportingTestDirectory"
            );

            if (Directory.Exists(
                savePath
            ))
            {
                Directory.Delete(
                    savePath,
                    true
                );
            }

            Directory.CreateDirectory(
                savePath
            );
        }

        [Fact]
        public async Task TestShouldSaveReportsToFileWhenReportRepositoryContainsReports()
        {
            // Given
            var reportId = "report-id";
            var expectedFileName = "Reporting_report-id.log";
            var filePath = Path.Combine(
                savePath,
                "Reporting",
                expectedFileName
            );
            var expectedMessageText = "Some Message Text to Help pinpoint it is added!";
            var dataText = "We will be passing in some text";
            var expectedDataText = $@"{dataText}";

            var serverInfoMock = new Mock<ServerInfo>();
            var reportRepositoryMock = new Mock<ReportRepository>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                savePath
            );

            reportRepositoryMock.Setup(
                mock => mock.TakeAll()
            ).Returns(
                new List<Report>
                {
                    new Report(
                        reportId
                    ).AddItem(
                        new ReportItem(
                            expectedMessageText,
                            dataText
                        )
                    )
                }
            );

            // When
            var handler = new SavePendingReportItemsEventHandler(
                serverInfoMock.Object,
                reportRepositoryMock.Object
            );
            await handler.Handle(
                new SavePendingReportItemsEvent(),
                CancellationToken.None
            );

            var actual = File.ReadAllText(
                filePath
            );

            // Then
            actual
                .Should()
                .ContainAll(
                    new string[]
                    {
                        expectedMessageText,
                        expectedDataText
                    }
                );
        }

        [Fact]
        public async Task TestShouldAppendReportsToFileWhenTheReportAlreadyExists()
        {
            // Given
            var reportId = "report-id";
            var expectedFileName = "Reporting_report-id.log";
            var filePath = Path.Combine(
                savePath,
                "Reporting",
                expectedFileName
            );
            var expectedMessageText1 = "Some Message Text to Help pinpoint it is added!";
            var expectedMessageText2 = "This is some other text that should help to check appending";
            var expectedDataText1 = "We will be passing in some text";
            var expectedDataText2 = "This is the second data text that should be in file";

            var serverInfoMock = new Mock<ServerInfo>();
            var reportRepositoryMock = new Mock<ReportRepository>();

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                savePath
            );

            reportRepositoryMock.Setup(
                mock => mock.TakeAll()
            ).Returns(
                new List<Report>
                {
                    new Report(
                        reportId
                    ).AddItem(
                        new ReportItem(
                            expectedMessageText1,
                            expectedDataText1
                        )
                    )
                }
            );

            // When
            var handler = new SavePendingReportItemsEventHandler(
                serverInfoMock.Object,
                reportRepositoryMock.Object
            );
            await handler.Handle(
                new SavePendingReportItemsEvent(),
                CancellationToken.None
            );

            var existing = File.ReadAllText(
                filePath
            );
            existing
                .Should()
                .ContainAll(
                    new string[]
                    {
                        expectedMessageText1,
                        expectedDataText1
                    }
                ).And
                .NotContainAll(
                    new string[]
                    {
                        expectedMessageText2,
                        expectedDataText2
                    }
                );

            // Update mock to add another different record
            reportRepositoryMock.Setup(
                mock => mock.TakeAll()
            ).Returns(
                new List<Report>
                {
                    new Report(
                        reportId
                    ).AddItem(
                        new ReportItem(
                            expectedMessageText2,
                            expectedDataText2
                        )
                    )
                }
            );

            // Call a second time to append new text
            await handler.Handle(
                new SavePendingReportItemsEvent(),
                CancellationToken.None
            );

            var actual = File.ReadAllText(
                filePath
            );

            // Then
            actual
                .Should()
                .ContainAll(
                    new string[]
                    {
                        expectedMessageText1,
                        expectedDataText1,
                        expectedMessageText2,
                        expectedDataText2
                    }
                );
        }
    }
}