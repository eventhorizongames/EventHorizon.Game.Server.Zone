using EventHorizon.Plugin.Zone.System.Combat.Events.Level;
using EventHorizon.TimerService;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Timer
{
    public class EntityLevelUpTimer : ITimerTask
    {
        public int Period { get; } = 10;
        public string Tag { get; } = "EntityLevelUp";
        public INotification OnRunEvent { get; } = new EntityLevelUpFromQueueEvent();
    }
}