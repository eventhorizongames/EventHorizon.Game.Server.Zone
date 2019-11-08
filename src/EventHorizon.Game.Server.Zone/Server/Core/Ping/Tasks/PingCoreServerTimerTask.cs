using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Events.Lifetime;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Server.Core.Ping.Tasks
{
    public class PingCoreServerTimerTask : ITimerTask
    {
        public int Period { get; } = 1000 * 30; // Every 30 Seconds
        public string Tag { get; } = "RunServerActions";
        public IRequest<bool> OnValidationEvent {get;} = new IsServerStarted();
        public INotification OnRunEvent { get; } = new PingCoreServerEvent();
    }
}