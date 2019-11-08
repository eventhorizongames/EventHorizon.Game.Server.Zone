using EventHorizon.Zone.System.Combat.Events.Level;
using EventHorizon.Zone.System.Combat.Events.Life;
using EventHorizon.TimerService;
using MediatR;
using EventHorizon.Zone.Core.Events.Lifetime;

namespace EventHorizon.Zone.System.Combat.Timer
{
    public class UpdateEntityLifeTimer : ITimerTask
    {
        public int Period { get; } = 10;
        public string Tag { get; } = "UpdateEntityLife";
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new UpdateEntityLifeFromQueueEvent();
    }
}