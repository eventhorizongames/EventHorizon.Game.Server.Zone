using MediatR;

namespace EventHorizon.TimerService
{
    public interface ITimerTask
    {
        int Period { get; }
        string Tag { get; }
        IRequest<bool> OnValidationEvent { get; }
        INotification OnRunEvent { get; }
    }
}