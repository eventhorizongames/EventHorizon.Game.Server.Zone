using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command.Respond;
using EventHorizon.Game.Server.Zone.Admin.Command.Scripts.Model;
using EventHorizon.Game.Server.Zone.Admin.Command.Scripts.State;
using EventHorizon.Zone.Core.Model.Admin;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Admin.Command.Scripts.Run
{
    public struct RunAdminCommandFromScriptHandler : INotificationHandler<AdminCommandEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly AdminCommandRepository _adminCommandRepository;
        public RunAdminCommandFromScriptHandler(
            ILogger<RunAdminCommandFromScriptHandler> logger,
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
            var commandScriptList = _adminCommandRepository.Where(
                request.Command.Command
            );

            foreach (var commandScript in commandScriptList)
            {
                try
                {
                    var response = await _mediator.Send(
                        new RunServerScriptCommand(
                            commandScript.ScriptFile,
                            new Dictionary<string, object>()
                            { 
                                { "Command", request.Command } 
                            }
                        )
                    ) as AdminCommandScriptResponse;
                        
                    await _mediator.Send(
                        new ResponseToAdminCommand(
                            request.ConnectionId,
                            new StandardAdminCommandResponse(
                                request.Command.Command,
                                request.Command.RawCommand,
                                response.Success,
                                response.Message
                            )
                        )
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Problem Running Command",
                        commandScript
                    );
                }
            }
        }
    }
}