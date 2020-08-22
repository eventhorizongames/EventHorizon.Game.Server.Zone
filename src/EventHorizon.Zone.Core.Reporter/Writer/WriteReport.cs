namespace EventHorizon.Zone.Core.Reporter.Writer
{
    using EventHorizon.Zone.Core.Reporter.Model;
    using MediatR;

    public struct WriteReport : IRequest
    {
        public Report Report { get; }

        public WriteReport(
            Report report
        )
        {
            Report = report;
        }
    }
}