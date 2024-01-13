namespace EventHorizon.Zone.Core.Reporter.Writer;

using EventHorizon.Zone.Core.Reporter.Model;

using MediatR;

public struct WriteReportToFile : IRequest
{
    public Report Report { get; }

    public WriteReportToFile(
        Report report
    )
    {
        Report = report;
    }
}
