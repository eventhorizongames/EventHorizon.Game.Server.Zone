namespace EventHorizon.Zone.Core.Reporter.Writer
{
    using EventHorizon.Zone.Core.Reporter.Model;

    using MediatR;

    public struct WriteReportToElasticsearch : IRequest
    {
        public Report Report { get; }

        public WriteReportToElasticsearch(
            Report report
        )
        {
            Report = report;
        }
    }
}
