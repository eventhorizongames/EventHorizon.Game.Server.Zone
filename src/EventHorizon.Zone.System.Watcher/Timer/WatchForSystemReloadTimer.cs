using EventHorizon.TimerService;
using EventHorizon.Zone.System.Watcher.Check;
using MediatR;

namespace EventHorizon.Zone.System.Watcher.Timer
{
    public class WatchForSystemReloadTimer : ITimerTask
    {
        public int Period { get; } = 5000;
        public string Tag { get; } = "WatchForSystemReload";
        public INotification OnRunEvent { get; } = new CheckPendingReloadEvent();
    }
}