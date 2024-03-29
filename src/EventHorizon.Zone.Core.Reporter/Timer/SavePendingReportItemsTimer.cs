namespace EventHorizon.Zone.Core.Reporter.Timer;

using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Reporter.Save;

using MediatR;

public class SavePendingReportItemsTimer
    : ITimerTask
{
    public int Period { get; } = 15000; // Every 15 Seconds
    public string Tag { get; } = "SavePendingReportItems";
    public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
    public INotification OnRunEvent { get; } = new SavePendingReportItemsEvent();
    public bool LogDetails { get; } = true;
}
