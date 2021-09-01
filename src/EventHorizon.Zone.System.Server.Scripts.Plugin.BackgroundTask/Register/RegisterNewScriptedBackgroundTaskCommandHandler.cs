namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Register
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Api;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class RegisterNewScriptedBackgroundTaskCommandHandler
        : IRequestHandler<RegisterNewScriptedBackgroundTaskCommand, StandardCommandResult>
    {
        private readonly BackgroundTaskWrapperRepository _repository;
        private readonly BackgroundTaskWrapperBuilder _builder;

        public RegisterNewScriptedBackgroundTaskCommandHandler(
            BackgroundTaskWrapperRepository repository,
            BackgroundTaskWrapperBuilder builder
        )
        {
            _repository = repository;
            _builder = builder;
        }

        public Task<StandardCommandResult> Handle(
            RegisterNewScriptedBackgroundTaskCommand request,
            CancellationToken cancellationToken
        )
        {
            if (_repository.TryRemove(
                request.BackgroundTask.Id,
                out var backgroundTaskWrapper
            ))
            {
                backgroundTaskWrapper.Dispose();
            }

            var newBackgroundTaskWrapper = _builder.Build(
                request.BackgroundTask
            );

            // Add Wrapper to State
            _repository.Add(
                request.BackgroundTask.Id,
                newBackgroundTaskWrapper
            );

            // Start new Task
            newBackgroundTaskWrapper.Start();

            return new StandardCommandResult()
                .FromResult();
        }
    }
}
