namespace EventHorizon.Game.Server.Zone.Admin.SystemWatcher.State
{
    public class SystemWatcherState : ISystemWatcherState
    {
        public bool PendingReload
        {
            get;
            set;
        } = false;

        public void SetToPendingReload()
        {
            this.PendingReload = true;
        }
        public void RemovePendingReload()
        {
            this.PendingReload = false;
        }
    }
}