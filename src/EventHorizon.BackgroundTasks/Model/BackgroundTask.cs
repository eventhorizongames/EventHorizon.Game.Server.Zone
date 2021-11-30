namespace EventHorizon.BackgroundTasks.Model;

using MediatR;

public interface BackgroundTask
    : IRequest<BackgroundTaskResult>
{
    string ReferenceId { get; }
}

public interface BackgroundTaskHandler<T>
    : IRequestHandler<T, BackgroundTaskResult>
    where T : BackgroundTask
{

}
