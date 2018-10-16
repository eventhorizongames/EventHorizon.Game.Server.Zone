using EventHorizon.Plugin.Zone.System.Combat.Events.Level;
using EventHorizon.Plugin.Zone.System.Combat.Events.Life;
using EventHorizon.TimerService;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Timer
{
    public class UpdateEntityLifeTimer : ITimerTask
    {
        public int Period { get; } = 10;
        public INotification OnRunEvent { get; } = new UpdateEntityLifeFromQueueEvent();
    }
}