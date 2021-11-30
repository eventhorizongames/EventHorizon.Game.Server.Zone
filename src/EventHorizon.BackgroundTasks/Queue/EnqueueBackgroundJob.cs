namespace EventHorizon.BackgroundTasks.Queue;

using EventHorizon.BackgroundTasks.Model;

using MediatR;

public class EnqueueBackgroundJob
    : IRequest<EnqueueBackgroundJobResult>
{
    public BackgroundTask Task { get; }

    public EnqueueBackgroundJob(
        BackgroundTask task
    )
    {
        Task = task;
    }
}
