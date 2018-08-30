using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Schedule;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Impl
{
    public class MoveRegisteredAgentsTimer : IMoveRegisteredAgentsTimer
    {
        private static long CALLBACK_IN_MILLSECONDS = 100;
        private Timer _moveRegisteredAgentsTimer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        public MoveRegisteredAgentsTimer(ILogger<MoveRegisteredAgentsTimer> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Start()
        {
            _moveRegisteredAgentsTimer = new Timer(this.OnMoveRegisteredAgents, new TimerState(), 0, CALLBACK_IN_MILLSECONDS);
        }

        public void Stop()
        {
            _moveRegisteredAgentsTimer.Dispose();
        }

        public void OnMoveRegisteredAgents(object state)
        {
            var timerState = (TimerState)state;
            if (timerState.IsRunning)
            {
                // Log that MoveRegister timer is still running
                _logger.LogWarning("Timer found that it was already running. Check for long running Move Registered Agent loop.");
                return;
            }
            timerState.IsRunning = true;
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {

                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new MoveRegisteredAgentsEvent()).GetAwaiter().GetResult();
            }
            timerState.IsRunning = false;
        }
        public class TimerState
        {
            public bool IsRunning { get; set; }
        }
    }
}