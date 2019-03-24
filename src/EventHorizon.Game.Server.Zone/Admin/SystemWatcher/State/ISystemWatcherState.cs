namespace EventHorizon.Game.Server.Zone.Admin.SystemWatcher.State
{
    public interface ISystemWatcherState
    {
        bool PendingReload { get; }
        void SetToPendingReload();
        void RemovePendingReload();
    }
}