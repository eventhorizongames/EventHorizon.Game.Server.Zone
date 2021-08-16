namespace EventHorizon.Zone.Core.Reporter.Tracker
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Reporter.Model;

    public class ReportTrackingRepository : ReportTracker, ReportRepository
    {
        private readonly ConcurrentDictionary<string, Report> _reports = new ConcurrentDictionary<string, Report>();

        private readonly IDateTimeService _dateTime;

        public ReportTrackingRepository(
            IDateTimeService dateTime
        )
        {
            _dateTime = dateTime;
        }

        public virtual void Clear(
            string id
        )
        {
            _reports.TryRemove(
                id,
                out _
            );
        }

        public virtual IEnumerable<Report> TakeAll()
        {
            var values = _reports.Values;
            _reports.Clear();
            return values;
        }

        public virtual void Track(
            string id,
            string correlationId,
            string message,
            object data
        )
        {
            var timestamp = _dateTime.Now;
            var reportItem = new ReportItem(
                correlationId,
                message,
                timestamp,
                data
            );
            _reports.AddOrUpdate(
                id,
                new Report(
                    id,
                    timestamp
                ).AddItem(
                    reportItem
                ),
                (_, current) => current.AddItem(
                    reportItem
                )
            );
        }
    }
}
