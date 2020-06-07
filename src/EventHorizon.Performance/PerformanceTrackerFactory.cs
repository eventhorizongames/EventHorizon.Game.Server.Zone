namespace EventHorizon.Performance
{
    public interface PerformanceTrackerFactory
    {
        PerformanceTracker Build(
            string trackerName
        );
    }
}