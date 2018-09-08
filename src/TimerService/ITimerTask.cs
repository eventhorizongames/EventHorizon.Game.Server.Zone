using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventHorizon.TimerService
{
    public interface ITimerTask
    {
        int Period { get; }
        INotification OnRunEvent { get; }
    }
}