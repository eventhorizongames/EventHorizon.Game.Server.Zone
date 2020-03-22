namespace EventHorizon.Zone.Core.Reporter.Tracker
{
    using System.Collections.Concurrent;
    using EventHorizon.Zone.Core.Reporter.Model;
    using System.Collections.Generic;
    using System;
    using System.Text.Json;

    public class ToRepositoryReportTracker : ReportTracker, ReportRepository
    {
        private static JsonSerializerOptions JSON_OPTIONS = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        private ConcurrentDictionary<string, Report> REPORTS = new ConcurrentDictionary<string, Report>();

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
            var reportItem = new ReportItem(
                $"{DateTime.UtcNow.ToString()} - {message}",
                JsonSerializer.Serialize(
                    data,
                    JSON_OPTIONS
                )
            );
            REPORTS.AddOrUpdate(
                id,
                new Report(
                    id
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