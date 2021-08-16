namespace EventHorizon.Zone.Core.Reporter.Model
{
    using System.Collections.Generic;

    public interface ReportRepository
    {
        IEnumerable<Report> TakeAll();
    }
}
