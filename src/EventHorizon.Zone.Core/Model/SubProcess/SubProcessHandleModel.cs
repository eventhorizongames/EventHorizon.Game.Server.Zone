namespace EventHorizon.Zone.Core.Model.SubProcess
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    public class SubProcessHandleModel
        : SubProcessHandle
    {
        private readonly Process _process;

        public SubProcessHandleModel(
            Process process
        )
        {
            _process = process;
        }

        public int ExitCode => _process.ExitCode;

        public Task WaitForExitAsync(
            CancellationToken cancellationToken
        ) => _process.WaitForExitAsync(
            cancellationToken
        );
    }
}
