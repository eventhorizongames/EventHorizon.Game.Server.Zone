using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Schedule;
using EventHorizon.TimerService;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Impl
{
    public class MoveRegisteredAgentsTimer : ITimerTask
    {
        public int Period { get; } = 100;
        public INotification OnRunEvent { get; } = new MoveRegisteredAgentsEvent();
    }
}