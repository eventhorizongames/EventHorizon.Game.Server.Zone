namespace EventHorizon.BackgroundTasks.Queue;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.BackgroundTasks.Api;
using EventHorizon.BackgroundTasks.Model;

using MediatR;

public class EnqueueBackgroundJobHandler
    : IRequestHandler<EnqueueBackgroundJob, EnqueueBackgroundJobResult>
{
    private readonly BackgroundJobs _backgroundJobs;

    public EnqueueBackgroundJobHandler(
        BackgroundJobs backgroundJobs
    )
    {
        _backgroundJobs = backgroundJobs;
    }

    public async Task<EnqueueBackgroundJobResult> Handle(
        EnqueueBackgroundJob request,
        CancellationToken cancellationToken
    )
    {
        await _backgroundJobs.QueueAsync(
            request.Task,
            cancellationToken
        );

        return new EnqueueBackgroundJobResult
        {
            Success = true,
        };
    }
}
