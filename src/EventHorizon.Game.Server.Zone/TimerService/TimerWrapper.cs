using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Schedule;
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

        public TimerWrapper(ILogger logger, IServiceScopeFactory serviceScopeFactory, ITimerTask timerTask)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _timerTask = timerTask;
        }

        public void Start()
        {
            _timer = new Timer(this.OnRunTask, new TimerState(), 0, this._timerTask.Period);
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }
        }

        public void OnRunTask(object state)
        {
            var timerState = (TimerState)state;
            if (timerState.IsRunning)
            {
                // Log that MoveRegister timer is still running
                _logger.LogWarning("Timer found that it was already running. Check for long running loop; Id: {Id} | Guid: {GUID} | StartDate: {StartDate:MM-dd-yyyy HH:mm:ss.fffffffzzz} | TimeRunning: {TimeRunning}", timerState.Id, timerState.Guid, timerState.StartDate, DateTime.UtcNow - timerState.StartDate);
                return;
            }

            // _logger.LogInformation("DELAYED: Timer found that it was already running. Check for long running loop; Id: {Id} | Guid: {GUID} | StartDate: {StartDate:MM-dd-yyyy HH:mm:ss.fffffffzzz} | TimeRunning: {TimeRunning}", timerState.Id, timerState.Guid, timerState.StartDate, DateTime.UtcNow - timerState.StartDate);

            lock (timerState.LOCK)
            {
                timerState.Guid = Guid.NewGuid();
                timerState.IsRunning = true;
                timerState.StartDate = DateTime.UtcNow;
                using (var serviceScope = _serviceScopeFactory.CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<IMediator>().Publish(
                        this._timerTask.OnRunEvent,
                        CancellationToken.None
                    ).GetAwaiter().GetResult();
                }
                if (DateTime.Now.Add(DateTime.UtcNow - timerState.StartDate).CompareTo(DateTime.Now.AddMilliseconds(_timerTask.Period)) > 0)
                {
                    _logger.LogWarning("Timer ran long; Id: {Id} | Guid: {GUID} | StartDate: {StartDate:MM-dd-yyyy HH:mm:ss.fffffffzzz} | TimeRunning: {TimeRunning}", timerState.Id, timerState.Guid, timerState.StartDate, DateTime.UtcNow - timerState.StartDate);
                }
                timerState.IsRunning = false;
                timerState.StartDate = DateTime.UtcNow;
            }
        }

        public class TimerState
        {
            public object LOCK { get; internal set; } = new object();
            public string Id { get; internal set; } = Guid.NewGuid().ToString();
            public Guid Guid { get; internal set; }
            public bool IsRunning { get; set; }
            public DateTime StartDate { get; set; }
        }
    }
}