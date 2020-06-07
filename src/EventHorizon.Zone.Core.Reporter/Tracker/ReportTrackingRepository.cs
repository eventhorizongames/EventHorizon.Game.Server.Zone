namespace EventHorizon.Zone.Core.Reporter.Tracker
{
    using System.Collections.Concurrent;
    using EventHorizon.Zone.Core.Reporter.Model;
    using System.Collections.Generic;
    using System.Text.Json;
    using EventHorizon.Zone.Core.Model.DateTimeService;

    public class ReportTrackingRepository : ReportTracker, ReportRepository
    {
        private static JsonSerializerOptions JSON_OPTIONS = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        private ConcurrentDictionary<string, Report> REPORTS = new ConcurrentDictionary<string, Report>();

        private readonly IDateTimeService _dateTime;

        public ReportTrackingRepository(
            IDateTimeService dateTime
        )
        {
            _dateTime = dateTime;
        }

        public void Clear(
            string id
        )
        {
            REPORTS.TryRemove(
                id,
                out _
            );
        }

        public IEnumerable<Report> TakeAll()
        {
            var values = REPORTS.Values;
            REPORTS.Clear();
            return values;
        }

        public void Track(
            string id,
            string message,
            object data
        )
        {
            var timestamp = _dateTime.Now;
            var reportItem = new ReportItem(
                $"{timestamp.ToString()} - {message}",
                JsonSerializer.Serialize(
                    data,
                    JSON_OPTIONS
                )
            );
            REPORTS.AddOrUpdate(
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