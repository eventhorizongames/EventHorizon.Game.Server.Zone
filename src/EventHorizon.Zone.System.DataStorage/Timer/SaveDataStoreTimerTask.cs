namespace EventHorizon.Zone.System.DataStorage.Timer
{
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.System.DataStorage.Save;
    using MediatR;

    public class SaveDataStoreTimerTask 
        : ITimerTask
    {
        public int Period { get; } = 1000 * 30; // Every 30 Seconds
        public string Tag { get; } = nameof(SaveDataStoreTimerTask);
        public IRequest<bool> OnValidationEvent { get; } = new IsServerStarted();
        public INotification OnRunEvent { get; } = new RunSaveDataStoreEvent();
        public bool LogDetails { get; } = true;
    }
}