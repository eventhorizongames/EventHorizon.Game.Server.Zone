namespace EventHorizon.Performance.Model
{
    public sealed class EmptyPerformanceTracker : PerformanceTracker
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Design",
            "CA1063:Implement IDisposable Correctly",
            Justification = "No managed resources, is empty."
        )]
        public void Dispose()
        {
        }
    }
}
