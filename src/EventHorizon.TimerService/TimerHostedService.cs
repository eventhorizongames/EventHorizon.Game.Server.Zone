
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventHorizon.TimerService
{
    public class TimerHostedService : HostedService
    {
        private readonly List<TimerWrapper> _timerTasks = new List<TimerWrapper>();

        public TimerHostedService(
            ILoggerFactory loggerFactory, 
            IServiceScopeFactory serviceScopeFactory, 
            IEnumerable<ITimerTask> timerTasks
        )
        {
            foreach (var timerTask in timerTasks)
            {
                _timerTasks.Add(
                    new TimerWrapper(
                        loggerFactory.CreateLogger(
                            nameof(timerTask)
                        ),
                        serviceScopeFactory,
                        timerTask
                    )
                );
            }
        }

        protected override Task ExecuteAsync(
            CancellationToken cancellationToken
        )
        {
            _timerTasks.ForEach(
                timerWrapper => timerWrapper.Start()
            );
            cancellationToken.Register(
                this.StopTimerTasks
            );
            return Task.CompletedTask;
        }
        private void StopTimerTasks()
        {
            _timerTasks.ForEach(
                timerTask => timerTask.Stop()
            );
        }
    }
}