using EventHorizon.Game.Server.Zone.Admin.SystemWatcher.Check;
using EventHorizon.Zone.System.Combat.Events.Level;
using EventHorizon.TimerService;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Admin.SystemWatcher.Timer
{
    public class WatcherReloadSystemTimer : ITimerTask
    {
        public int Period { get; } = 5000;
        public string Tag { get; } = "WatcherReloadSystem";
        public INotification OnRunEvent { get; } = new CheckForSystemReloadEvent();
    }
}