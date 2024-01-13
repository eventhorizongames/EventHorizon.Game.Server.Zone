namespace EventHorizon.Zone.System.Admin.Plugin.Command.Run;

using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
using EventHorizon.Zone.System.Admin.Plugin.Command.State;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;

using global::System;
using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

public class RunAdminCommandScriptHandler : INotificationHandler<AdminCommandEvent>
{
    private const string INVALID_RUN_ERROR_CODE = "base_script_response";

    readonly ILogger _logger;
    readonly IMediator _mediator;
    readonly AdminCommandRepository _adminCommandRepository;
    public RunAdminCommandScriptHandler(
        ILogger<RunAdminCommandScriptHandler> logger,
        IMediator mediator,
        AdminCommandRepository adminCommandRepository
    )
    {
        _logger = logger;
        _mediator = mediator;
        _adminCommandRepository = adminCommandRepository;
    }

    public async Task Handle(
        AdminCommandEvent request,
        CancellationToken cancellationToken
    )
    {
        var commandList = _adminCommandRepository.Where(
            request.Command.Command
        );

        foreach (var commandInstance in commandList)
        {
            try
            {
                _logger.LogDebug(
                    "Running Admin Command Script {Command} | {ScriptFile}",
                    commandInstance.Command,
                    commandInstance.ScriptFile
                );
                var response = await _mediator.Send(
                    new RunServerScriptCommand(
                        commandInstance.ScriptFile,
                        new Dictionary<string, object>()
                        {
                            { "Command", request.Command },
                            { "Data", request.Data },
                        }
                    )
                ) as AdminCommandScriptResponse;

                await _mediator.Send(
                    new RespondToAdminCommand(
                        request.ConnectionId,
                        new StandardAdminCommandResponse(
                            request.Command.Command,
                            request.Command.RawCommand,
                            response?.Success ?? false,
                            response?.Message ?? INVALID_RUN_ERROR_CODE
                        )
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Problem Running Command",
                    commandInstance
                );
            }
        }
    }
}
