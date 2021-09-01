namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Remove
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Api;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class RemoveScriptedBackgroundTaskCommandHandler
        : IRequestHandler<RemoveScriptedBackgroundTaskCommand, StandardCommandResult>
    {
        private readonly BackgroundTaskWrapperRepository _repository;

        public RemoveScriptedBackgroundTaskCommandHandler(
            BackgroundTaskWrapperRepository repository
        )
        {
            _repository = repository;
        }

        public Task<StandardCommandResult> Handle(
            RemoveScriptedBackgroundTaskCommand request,
            CancellationToken cancellationToken
        )
        {
            if(_repository.TryRemove(
                request.TaskId,
                out var backgroundTaskWrapper
            ))
            {
                backgroundTaskWrapper.Dispose();
            }

            return new StandardCommandResult()
                .FromResult();
        }
    }
}
