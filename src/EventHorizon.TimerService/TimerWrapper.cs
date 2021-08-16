namespace EventHorizon.TimerService
{
    using System;
    using System.Text;
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
                OnRunTask,
                new TimerState(),
                0,
                _timerTask.Period
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
                LogMessage(
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

                if (_timerTask.LogDetails)
                {
                    LogMessage(
                        "Starting timer run",
                        timerState
                    );
                }

                using var serviceScope = _serviceScopeFactory.CreateScope();
                var mediator = serviceScope.ServiceProvider.GetService<IMediator>();

                if (_timerTask.OnValidationEvent != null
                    && !await mediator.Send(
                        _timerTask.OnValidationEvent
                    )
                )
                {
                    // The validation failed for the Task, do not Publish Event.
                    return;
                }

                await mediator.Publish(
                    _timerTask.OnRunEvent
                );
            }
            catch (Exception ex)
            {
                timerState.ErrorsCaught += 1;
                LogMessage(
                    "Timer caught an Exception.",
                    timerState,
                    ex
                );
            }
            finally
            {
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
                    LogMessage(
                        "Timer ran long.",
                        timerState
                    );
                }

                if (_timerTask.LogDetails)
                {
                    LogMessage(
                        "Finished timer run",
                        timerState
                    );
                }

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
            message = new StringBuilder(
                message
            ).Append(
                "\n TimerState: "
            ).Append(
                "\n | Id: {Id}"
            ).Append(
                "\n | Guid: {GUID}"
            ).Append(
                "\n | Tag: {Tag}"
            ).Append(
                "\n | StartDate: {StartDate:MM-dd-yyy HH:mm:ss.fffffffzzz}"
            ).Append(
                "\n | FinishedDate: {FinishedDate:MM-dd-yyy HH:mm:ss.fffffffzzz}"
            ).Append(
                "\n | TimeRunning: {TimeRunning}"
            ).Append(
                "\n | TimerRunningTicks: {TimerRunningTicks}"
            ).Append(
                "\n | TimerState: {@TimerTask}"
            ).ToString();

            if (ex != null)
            {
                _logger.LogError(
                    ex,
                    message,
                    logArgs
                );
                return;
            }

            _logger.LogWarning(
                message,
                logArgs
            );
        }
    }

    public class TimerState
    {
        public SemaphoreSlim LOCK { get; } = new SemaphoreSlim(1, 1);
        public string Id { get; } = Guid.NewGuid().ToString();
        public Guid Guid { get; internal set; }
        public bool IsRunning { get; set; }
        public DateTime StartDate { get; set; }
        public int ErrorsCaught { get; set; }
    }
}
