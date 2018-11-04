using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.ServerAction.Run;
using EventHorizon.Schedule;
using EventHorizon.TimerService;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.ServerAction.Timer
{
    public class RunServerActionsTimerTask : ITimerTask
    {
        public int Period { get; } = 10;
        public INotification OnRunEvent { get; } = new RunPendingServerActionsEvent();
    }
}