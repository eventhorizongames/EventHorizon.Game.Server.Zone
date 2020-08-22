namespace EventHorizon.TimerService
{
    using MediatR;

    public interface ITimerTask
    {
        int Period { get; }
        string Tag { get; }
        IRequest<bool> OnValidationEvent { get; }
        INotification OnRunEvent { get; }
    }
}