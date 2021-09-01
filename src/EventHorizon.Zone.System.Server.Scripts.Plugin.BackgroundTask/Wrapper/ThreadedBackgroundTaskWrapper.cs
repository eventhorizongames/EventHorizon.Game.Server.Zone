namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Wrapper
{
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Api;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Model;

    using global::System;
    using global::System.Text;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    // TODO: On upgrade to .NET6 an async/await timer:
    // https://www.infoq.com/news/2021/08/net6-Threading/
    public class ThreadedBackgroundTaskWrapper
        : BackgroundTaskWrapper
    {
        private readonly BackgroundTaskWrapperState _state;
        private Timer? _timer;

        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ScriptedBackgroundTask _task;

        public ThreadedBackgroundTaskWrapper(
            ILogger logger,
            IServiceScopeFactory serviceScopeFactory,
            ScriptedBackgroundTask task
        )
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _task = task;

            _state = new BackgroundTaskWrapperState();
        }

        public void Start()
        {
            _timer = new Timer(
                OnRunTask,
                _state,
                0,
                _task.TaskPeriod
            );
        }

        public void Resume()
        {
            _state.IsStopped = false;
        }

        public void Stop()
        {
            _state.IsStopped = true;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public void OnRunTask(
            object? state
        )
        {
            if (state.IsNull())
            {
                _logger.LogError(
                    "Scripted Background Task had an invalid state: {ScriptedBackgroundTaskId}.",
                    _task.Id
                );
                Dispose();
                return;
            }

            OnRunTaskAsync(
                state
            ).GetAwaiter().GetResult();
        }

        private async Task OnRunTaskAsync(
            object state
        )
        {
            var taskState = (BackgroundTaskWrapperState)state;
            if (taskState.IsStopped)
            {
                return;
            }
            else if (taskState.IsRunning)
            {
                LogMessage(
                    "Scripted Background Task found that it was already running.",
                    taskState
                );
                return;
            }

            if (!await taskState.LOCK.WaitAsync(0))
            {
                return;
            }

            try
            {
                taskState.Guid = Guid.NewGuid();
                taskState.IsRunning = true;
                taskState.StartDate = DateTime.UtcNow;

                using var serviceScope = _serviceScopeFactory.CreateScope();
                var scriptServices = serviceScope.ServiceProvider
                    .GetRequiredService<ServerScriptServices>();

                await _task.TaskTrigger(
                    scriptServices
                );
            }
            catch (Exception ex)
            {
                taskState.ErrorsCaught += 1;
                LogMessage(
                    "Scripted Background Task caught an Exception.",
                    taskState,
                    ex
                );
            }
            finally
            {
                if (
                    DateTime.UtcNow.Add(
                        DateTime.UtcNow - taskState.StartDate
                    ).CompareTo(
                        DateTime.UtcNow.AddMilliseconds(
                            _task.TaskPeriod
                        )
                    ) > 0
                )
                {
                    LogMessage(
                        "Scripted Background Task ran long.",
                        taskState
                    );
                }

                taskState.IsRunning = false;
                taskState.StartDate = DateTime.UtcNow;
                taskState.LOCK.Release();
            }
        }

        private void LogMessage(
            string message,
            BackgroundTaskWrapperState state,
            Exception? ex = null
        )
        {
            var timeRunning = DateTime.UtcNow - state.StartDate;
            var timeRunningTicks = timeRunning.Ticks;
            var logArgs = new object[]
            {
                state.Id,
                state.Guid,
                _task.TaskTags,
                state.StartDate,
                DateTime.UtcNow,
                timeRunning,
                timeRunningTicks,
                _task
            };
            message = new StringBuilder(
                message
            ).Append(
                "\n BackgroundTaskWrapperState: "
            ).Append(
                "\n | Id: {Id}"
            ).Append(
                "\n | Guid: {GUID}"
            ).Append(
                "\n | Tags: {Tags}"
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

    internal class BackgroundTaskWrapperState
    {
        public SemaphoreSlim LOCK { get; } = new SemaphoreSlim(1, 1);
        public string Id { get; } = Guid.NewGuid().ToString();
        public Guid Guid { get; internal set; }
        public bool IsStopped { get; set; }
        public bool IsRunning { get; set; }
        public DateTime StartDate { get; set; }
        public int ErrorsCaught { get; set; }
    }
}
