namespace EventHorizon.BackgroundTasks.Api;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.BackgroundTasks.Model;

public interface BackgroundJobs
{
    ValueTask QueueAsync(
        BackgroundTask workItem,
        CancellationToken cancellationToken
    );

    ValueTask<BackgroundTask> DequeueAsync(
        CancellationToken cancellationToken
    );
}
