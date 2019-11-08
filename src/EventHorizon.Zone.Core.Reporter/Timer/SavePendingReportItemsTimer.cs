using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Reporter.Save;
using MediatR;

namespace EventHorizon.Zone.Core.Reporter.Timer
{
    public class SavePendingReportItemsTimer : ITimerTask
    {
        public int Period { get; } = 15000;
        public string Tag { get; } = "SavePendingReportItems";
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new SavePendingReportItemsEvent();
    }
}