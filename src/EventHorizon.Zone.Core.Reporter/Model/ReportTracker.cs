namespace EventHorizon.Zone.Core.Reporter.Model
{
    public interface ReportTracker
    {
        void Clear(
            string id
        );
        void Track(
            string id,
            string correlationId,
            string message,
            object data
        );
    }
}
