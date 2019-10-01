namespace EventHorizon.Performance
{
    public interface IPerformanceTracker
    {
        ITrackerInstance Track(string trackerName);
    }
}