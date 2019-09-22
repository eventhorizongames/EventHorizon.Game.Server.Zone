using EventHorizon.Zone.System.Combat.Events.Level;
using EventHorizon.Zone.System.Combat.Events.Life;
using EventHorizon.TimerService;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Timer
{
    public class UpdateEntityLifeTimer : ITimerTask
    {
        public int Period { get; } = 10;
        public string Tag { get; } = "UpdateEntityLife";
        public INotification OnRunEvent { get; } = new UpdateEntityLifeFromQueueEvent();
    }
}