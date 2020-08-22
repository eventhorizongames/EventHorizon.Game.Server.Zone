namespace EventHorizon.TimerService
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class TimerWrapper
    {
        private Timer _timer;

        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ITimerTask _timerTask;

        public TimerWrapper(
            ILogger logger,
            IServiceScopeFactory serviceScopeFactory,
            ITimerTask timerTask
        )
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _timerTask = timerTask;
        }

        public void Start()
        {
            _timer = new Timer(
                this.OnRunTask,
                new TimerState(),
                0,
                this._timerTask.Period
            );
        }

        public void Stop()
        {
            _timer?.Dispose();
        }


        public void OnRunTask(
            object state
        )
        {
            OnRunTaskAsync(
                state
            ).GetAwaiter().GetResult();
        }

        private async Task OnRunTaskAsync(
            object state
        )
        {
            var timerState = (TimerState)state;
            if (timerState.IsRunning)
            {
                // Log that MoveRegister timer is still running
                this.LogMessage(
                    "Timer found that it was already running.",
                    timerState
                );
                return;
            }

            if (!await timerState.LOCK.WaitAsync(0))
            {
                return;
            }
            try
            {
                timerState.Guid = Guid.NewGuid();
                timerState.IsRunning = true;
                timerState.StartDate = DateTime.UtcNow;
                using (var serviceScope = _serviceScopeFactory.CreateScope())
                {
                    try
                    {
                        var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
                        if (this._timerTask.OnValidationEvent != null)
                        {
                            if (!await mediator.Send(
                                this._timerTask.OnValidationEvent
                            ))
                            {
                                return;
                            }
                            await mediator.Publish(
                                this._timerTask.OnRunEvent
                            );
                        }
                        else
                        {
                            mediator.Publish(
                                this._timerTask.OnRunEvent
                            ).GetAwaiter().GetResult();
                        }
                    }
                    catch (
                        Exception ex
                    )
                    {
                        this.LogMessage(
                            "Timer ran long.",
                            timerState,
                            ex
                        );
                    }
                }
                if (
                    DateTime.UtcNow.Add(
                        DateTime.UtcNow - timerState.StartDate
                    ).CompareTo(
                        DateTime.UtcNow.AddMilliseconds(
                            _timerTask.Period
                        )
                    ) > 0
                )
                {
                    this.LogMessage(
                        "Timer ran long.",
                        timerState
                    );
                }
            }
            finally
            {
                timerState.IsRunning = false;
                timerState.StartDate = DateTime.UtcNow;
                timerState.LOCK.Release();
            }
        }

        private void LogMessage(
            string message,
            TimerState state,
            Exception ex = null
        )
        {
            var timeRunning = DateTime.UtcNow - state.StartDate;
            var timeRunningTicks = timeRunning.Ticks;
            var logArgs = new object[]
            {
                state.Id,
                state.Guid,
                _timerTask.Tag,
                state.StartDate,
                DateTime.UtcNow,
                timeRunning,
                timeRunningTicks,
                _timerTask
            };
            message += " \nCheck for long running loop. \nTimerState: \n | Id: {Id} \n | Guid: {GUID} \n | Tag: {Tag} \n | StartDate: {StartDate:MM-dd-yyyy HH:mm:ss.fffffffzzz} \n | FinishedDate: {FinishedDate:MM-dd-yyyy HH:mm:ss.fffffffzzz} \n | TimeRunning: {TimeRunning} \n | TimeRunningTicks: {TimeRunningTicks} \n | TimerTask: {@TimerTask}";
            if (ex != null)
            {
                _logger.LogError(
                    ex,
                    message,
                    logArgs
                );
            }
            else
            {
                _logger.LogWarning(
                    message,
                    logArgs
                );
            }
        }
    }

    public class TimerState
    {
        public SemaphoreSlim LOCK { get; } = new SemaphoreSlim(1, 1);
        public string Id { get; } = Guid.NewGuid().ToString();
        public Guid Guid { get; internal set; }
        public bool IsRunning { get; set; }
        public DateTime StartDate { get; set; }
    }
}