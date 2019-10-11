using MediatR;

namespace EventHorizon.TimerService
{
    public interface ITimerTask
    {
        int Period { get; }
        string Tag { get; }
        INotification OnRunEvent { get; }
    }
}