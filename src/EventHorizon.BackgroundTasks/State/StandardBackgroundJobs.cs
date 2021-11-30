namespace EventHorizon.BackgroundTasks.State;

using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using EventHorizon.BackgroundTasks.Api;
using EventHorizon.BackgroundTasks.Model;

public class StandardBackgroundJobs
    : BackgroundJobs
{
    private readonly Channel<BackgroundTask> _queue;

    public StandardBackgroundJobs()
    {
        var capacity = 100;
        // Capacity should be set based on the expected application load and
        // number of concurrent threads accessing the queue.            
        // BoundedChannelFullMode.Wait will cause calls to WriteAsync() to return a task,
        // which completes only when space became available. This leads to backpressure,
        // in case too many publishers/calls start accumulating.
        var options = new BoundedChannelOptions(
            capacity
        )
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<BackgroundTask>(
            options
        );
    }

    public async ValueTask<BackgroundTask> DequeueAsync(
        CancellationToken cancellationToken
    )
    {
        return await _queue.Reader.ReadAsync(
            cancellationToken
        );
    }

    public async ValueTask QueueAsync(
        BackgroundTask workItem,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(workItem);

        await _queue.Writer.WriteAsync(
            workItem,
            cancellationToken
        );
    }
}
