namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Builders
{
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Api;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Model;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Wrapper;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class ThreadedBackgroundTaskWrapperBuilder
        : BackgroundTaskWrapperBuilder
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ThreadedBackgroundTaskWrapperBuilder(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory serviceScopeFactory
        )
        {
            _loggerFactory = loggerFactory;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public BackgroundTaskWrapper Build(
            ScriptedBackgroundTask task
        )
        {
            return new ThreadedBackgroundTaskWrapper(
                _loggerFactory.CreateLogger(
                    $"ThreadedBackgroundTaskWrapper.{task.Id}"
                ),
                _serviceScopeFactory,
                task
            );
        }
    }
}
