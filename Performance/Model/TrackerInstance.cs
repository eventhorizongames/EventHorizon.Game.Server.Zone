using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Performance.Model
{
    public class TrackerInstance : ITrackerInstance
    {
        readonly ILogger _logger;
        readonly Stopwatch watch;
        public TrackerInstance(ILogger logger)
        {
            _logger = logger;
            _logger.LogInformation("Starting Performance Tracking");
            watch = Stopwatch.StartNew();
        }
        public virtual void Dispose()
        {
            watch.Stop();
            _logger.LogInformation("Finished Performance Tracking, took: {0}ms", watch.ElapsedMilliseconds);
        }
    }
}