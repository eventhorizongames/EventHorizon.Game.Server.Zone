using System.Threading;
using System.Threading.Tasks;

namespace EventHorizon.Schedule
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}