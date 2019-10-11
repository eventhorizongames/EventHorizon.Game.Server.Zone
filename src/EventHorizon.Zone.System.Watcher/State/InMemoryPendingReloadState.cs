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
            this.IsPending = true;
        }
        public void RemovePending()
        {
            this.IsPending = false;
        }
    }
}