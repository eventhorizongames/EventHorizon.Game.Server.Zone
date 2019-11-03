using System.Collections.Generic;

namespace EventHorizon.Zone.Core.Reporter.Model
{
    public interface ReportRepository
    {
        IEnumerable<Report> TakeAll();
    }
}