namespace EventHorizon.Performance.Model
{
    using Microsoft.Extensions.Logging;

    public class ToLoggerPerformanceTrackerFactory : PerformanceTrackerFactory
    {
        private static readonly PerformanceTracker EMPTY_TRACKER = new EmptyPerformanceTracker();

        private readonly ILoggerFactory _loggerFactory;
        private readonly PerformanceSettings _settings;

        public ToLoggerPerformanceTrackerFactory(
            ILoggerFactory loggerfactory,
            PerformanceSettings settings
        )
        {
            _loggerFactory = loggerfactory;
            _settings = settings;
        }

        public PerformanceTracker Build(
            string trackerName
        )
        {
            if (!_settings.IsEnabled)
            {
                return EMPTY_TRACKER;
            }
            return new DetailsToLoggerPerformanceTracker(
                _loggerFactory.CreateLogger(
                    trackerName
                )
            );
        }
    }
}
