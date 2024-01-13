namespace EventHorizon.Zone.System.Watcher.State;

public interface PendingReloadState
{
    bool IsPending { get; }
    void SetToPending();
    void RemovePending();
}
