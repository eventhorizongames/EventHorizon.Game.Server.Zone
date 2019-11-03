using System.Collections.Concurrent;
using EventHorizon.Zone.Core.Reporter.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace EventHorizon.Zone.Core.Reporter.Tracker
{
    public class ToRepositoryReportTracker : ReportTracker, ReportRepository
    {
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
                JsonConvert.SerializeObject(
                    data,
                    Formatting.Indented
                )
            );
            REPORTS.AddOrUpdate(
                id,
                new Report(
                    id
                ).AddItem(
                    reportItem
                ),
                (key, current) => current.AddItem(
                    reportItem
                )
            );
        }
    }
}