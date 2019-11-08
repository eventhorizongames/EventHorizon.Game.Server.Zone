using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventHorizon.TimerService
{
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
                _logger.LogWarning(
                    "Timer found that it was already running. \nCheck for long running loop. \nTimerState: \n | Id: {Id} \n | Guid: {GUID} \n | Tag: {Tag} \n | StartDate: {StartDate:MM-dd-yyyy HH:mm:ss.fffffffzzz} \n | TimeRunning: {TimeRunning}",
                    timerState.Id,
                    timerState.Guid,
                    _timerTask.Tag,
                    timerState.StartDate,
                    DateTime.UtcNow - timerState.StartDate
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
                            if (await mediator.Send(
                                this._timerTask.OnValidationEvent
                            ))
                            {
                                await mediator.Publish(
                                    this._timerTask.OnRunEvent
                                );
                            }
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
                        _logger.LogError(ex,
                            "Timer Exception. \nTimerState: \n | Id: {Id} \n | Guid: {GUID} \n | Tag: {Tag} \n | StartDate: {StartDate:MM-dd-yyyy HH:mm:ss.fffffffzzz} \n | TimeRunning: {TimeRunning}",
                            timerState.Id,
                            timerState.Guid,
                            _timerTask.Tag,
                            timerState.StartDate,
                            DateTime.UtcNow - timerState.StartDate
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
                    _logger.LogWarning(
                        "Timer ran long. \nTimerState: \n | Id: {Id} \n | Guid: {GUID} \n | Tag: {Tag} \n | StartDate: {StartDate:MM-dd-yyyy HH:mm:ss.fffffffzzz} \n | TimeRunning: {TimeRunning}",
                        timerState.Id,
                        timerState.Guid,
                        _timerTask.Tag,
                        timerState.StartDate,
                        DateTime.UtcNow - timerState.StartDate
                    );
                }
                timerState.IsRunning = false;
                timerState.StartDate = DateTime.UtcNow;
            }
            finally
            {
                timerState.LOCK.Release();
            }
        }
    }

    public class TimerState
    {
        public SemaphoreSlim LOCK { get; internal set; } = new SemaphoreSlim(1, 1);
        public string Id { get; internal set; } = Guid.NewGuid().ToString();
        public Guid Guid { get; internal set; }
        public bool IsRunning { get; set; }
        public DateTime StartDate { get; set; }
    }
}