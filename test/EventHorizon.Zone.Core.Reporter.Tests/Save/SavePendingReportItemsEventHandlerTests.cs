namespace EventHorizon.Zone.Core.Reporter.Tests.Save;

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
using EventHorizon.Zone.Core.Reporter.Writer;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class SavePendingReportItemsEventHandlerTests
{
    [Fact]
    public async Task ShouldSendWriteReportEventWhenTakeAllContainsReports()
    {
        // Given
        var report1 = new Report();
        var report2 = new Report();

        var reports = new List<Report>()
        {
            report1,
            report2,
        };

        var mediatorMock = new Mock<IMediator>();
        var reportRepositoryMock = new Mock<ReportRepository>();

        reportRepositoryMock.Setup(
            mock => mock.TakeAll()
        ).Returns(
            reports
        );

        // When
        var handler = new SavePendingReportItemsEventHandler(
            mediatorMock.Object,
            reportRepositoryMock.Object
        );
        await handler.Handle(
            new SavePendingReportItemsEvent(),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Send(
                new WriteReport(
                    report1
                ),
                CancellationToken.None
            )
        );
        mediatorMock.Verify(
            mock => mock.Send(
                new WriteReport(
                    report2
                ),
                CancellationToken.None
            )
        );

    }
}
