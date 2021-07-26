namespace EventHorizon.Zone.System.ClientAssets.Timer
{
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.System.ClientAssets.Save;
    using MediatR;

    public class SaveClientAssetsTimerTask
        : ITimerTask
    {
        public int Period { get; } = 1000 * 30;
        public string Tag { get; } = nameof(SaveClientAssetsTimerTask);
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new RunSaveClientAssetsEvent();
        public bool LogDetails { get; }
    }
}
