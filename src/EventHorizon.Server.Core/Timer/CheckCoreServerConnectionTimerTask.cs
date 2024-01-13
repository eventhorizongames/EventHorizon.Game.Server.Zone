namespace EventHorizon.Server.Core.Timer;

using EventHorizon.Server.Core.Events.Check;
using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Events.Lifetime;

using MediatR;

public class CheckCoreServerConnectionTimerTask
    : ITimerTask
{
    public int Period { get; } = 5000; // Every 5 Seconds
    public string Tag { get; } = "CheckCoreServerConnection";
    public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
    public INotification OnRunEvent { get; } = new CheckCoreServerConnection();
    public bool LogDetails { get; } = true;
}
