namespace EventHorizon.Zone.Core.SubProcess.Model
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Model.SubProcess;

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
