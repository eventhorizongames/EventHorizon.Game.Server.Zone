namespace EventHorizon.Performance.Model
{
    using System.Diagnostics;

    using Microsoft.Extensions.Logging;

    public sealed class DetailsToLoggerPerformanceTracker : PerformanceTracker
    {
        private readonly ILogger _logger;
        private readonly Stopwatch watch;

        public DetailsToLoggerPerformanceTracker(
            ILogger logger
        )
        {
            _logger = logger;
            _logger.LogInformation(
                "Starting Performance Tracking"
            );
            watch = Stopwatch.StartNew();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Design",
            "CA1063:Implement IDisposable Correctly",
            Justification = "not disposing of managed resources"
        )]
        public void Dispose()
        {
            watch.Stop();
            _logger.LogInformation(
                "Finished Performance Tracking, Details: \n | {ElapsedMilliseconds} ms \n | {ElapsedTicks} ticks ",
                watch.ElapsedMilliseconds,
                watch.ElapsedTicks
            );
        }
    }
}
