using EventHorizon.Performance.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Performance.Impl
{
    public class PerformanceTracker : IPerformanceTracker
    {
        private static readonly ITrackerInstance EMPTY_TRACKER = new EmptyTrackerInstance();
        readonly IConfiguration _configuration;
        readonly ILoggerFactory _loggerFactory;
        public PerformanceTracker(IConfiguration configuration, ILoggerFactory loggerfactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerfactory;
        }
        public ITrackerInstance Track(string trackerName)
        {
            if (_configuration["DisablePerformanceTracker"] != null)
            {
                return EMPTY_TRACKER;
            }
            return new TrackerInstance(_loggerFactory.CreateLogger(trackerName));
        }
    }
}