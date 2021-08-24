namespace EventHorizon.Zone.System.Watcher.State
{
    public class InMemoryPendingReloadState : PendingReloadState
    {
        public bool IsPending
        {
            get;
            set;
        } = false;

        public void SetToPending()
        {
            IsPending = true;
        }
        public void RemovePending()
        {
            IsPending = false;
        }
    }
}
