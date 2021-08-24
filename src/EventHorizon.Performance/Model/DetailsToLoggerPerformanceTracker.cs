namespace EventHorizon.Performance.Model
{
    using System.Diagnostics;

    using Microsoft.Extensions.Logging;

    public sealed class DetailsToLoggerPerformanceTracker : PerformanceTracker
    {
        private readonly ILogger _logger;
        private readonly Stopwatch _watch;

        public DetailsToLoggerPerformanceTracker(
            ILogger logger
        )
        {
            _logger = logger;
            _logger.LogInformation(
                "Starting Performance Tracking"
            );
            _watch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _watch.Stop();
            _logger.LogInformation(
                "Finished Performance Tracking, Details: \n | {ElapsedMilliseconds} ms \n | {ElapsedTicks} ticks ",
                _watch.ElapsedMilliseconds,
                _watch.ElapsedTicks
            );
        }
    }
}
