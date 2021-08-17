namespace EventHorizon.Zone.Core.Reporter.Tracker
{
    using System;
    using System.Collections.Generic;

    using EventHorizon.Zone.Core.Reporter.Model;

    public class ReportTrackingRepositoryBySettings
        : ReportTracker,
        ReportRepository
    {
        private readonly ReportTrackingRepository _reportTrackingRepository;
        private readonly ReporterSettings _reporterSettings;

        public ReportTrackingRepositoryBySettings(
            ReportTrackingRepository reportTrackingRepository,
            ReporterSettings reporterSettings
        )
        {
            _reportTrackingRepository = reportTrackingRepository;
            _reporterSettings = reporterSettings;
        }

        public void Clear(
            string id
        )
        {
            if (_reporterSettings.IsEnabled)
            {
                _reportTrackingRepository.Clear(
                    id
                );
            }
        }

        public IEnumerable<Report> TakeAll()
        {
            if (_reporterSettings.IsEnabled)
            {
                return _reportTrackingRepository.TakeAll();
            }

            return Array.Empty<Report>();
        }

        public void Track(
            string id,
            string correlationId,
            string message,
            object? data
        )
        {
            if (_reporterSettings.IsEnabled)
            {
                _reportTrackingRepository.Track(
                    id,
                    correlationId,
                    message,
                    data
                );
            }
        }
    }
}
