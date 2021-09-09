namespace EventHorizon.Zone.Core.SubProcess
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.SubProcess;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.SubProcess;
    using EventHorizon.Zone.Core.SubProcess.Model;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class StartSubProcessCommandHandler
        : IRequestHandler<StartSubProcessCommand, CommandResult<SubProcessHandle>>
    {
        private readonly ILogger _logger;

        public StartSubProcessCommandHandler(
            ILogger<StartSubProcessCommandHandler> logger
        )
        {
            _logger = logger;
        }

        public Task<CommandResult<SubProcessHandle>> Handle(
            StartSubProcessCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var process = Process.Start(
                    new ProcessStartInfo(
                        request.ApplicationFullName
                    )
                );
                if (process == null)
                {
                    return new CommandResult<SubProcessHandle>(
                        "SUB_PROCESS_NOT_VALID"
                    ).FromResult();
                }
                return new CommandResult<SubProcessHandle>(
                    new SubProcessHandleModel(
                        process
                    )
                ).FromResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to Create SubProcess. {ApplicationFullName} | {@CommandRequest}",
                    request.ApplicationFullName,
                    request
                );
                return new CommandResult<SubProcessHandle>(
                    "SUB_PROCESS_START_ERROR"
                ).FromResult();
            }
        }
    }
}
