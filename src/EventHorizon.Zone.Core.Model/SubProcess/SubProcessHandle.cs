namespace EventHorizon.Zone.Core.Model.SubProcess;

using System.Threading;
using System.Threading.Tasks;

public interface SubProcessHandle
{
    int ExitCode { get; }
    Task WaitForExitAsync(
        CancellationToken cancellationToken
    );
}
