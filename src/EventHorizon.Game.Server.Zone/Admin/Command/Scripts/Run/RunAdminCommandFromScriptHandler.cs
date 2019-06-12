using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command.Model;
using EventHorizon.Game.Server.Zone.Admin.Command.Respond;
using EventHorizon.Game.Server.Zone.Admin.Command.Scripts.Model;
using EventHorizon.Game.Server.Zone.Admin.Command.Scripts.State;
using EventHorizon.Game.Server.Zone.Admin.Server.State;
using EventHorizon.Game.Server.Zone.Events.Admin;
using EventHorizon.Game.Server.Zone.Model.Admin;
using EventHorizon.Game.Server.Zone.Server.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Admin.Command.Scripts.Run
{
    public struct RunAdminCommandFromScriptHandler : INotificationHandler<AdminCommandEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly IServerScriptServices _serverScriptServices;
        readonly AdminCommandRepository _adminCommandRepository;
        readonly ServerScriptRepository _serverScriptRepository;
        public RunAdminCommandFromScriptHandler(
            ILogger<RunAdminCommandFromScriptHandler> logger,
            IMediator mediator,
            IServerScriptServices serverScriptServices,
            AdminCommandRepository adminCommandRepository,
            ServerScriptRepository serverScriptRepository
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverScriptServices = serverScriptServices;
            _adminCommandRepository = adminCommandRepository;
            _serverScriptRepository = serverScriptRepository;
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
                    var response = await _serverScriptRepository.Find(
                        commandScript.ScriptFile
                    ).Run<AdminCommandScriptResponse>(
                        _serverScriptServices,
                        new Dictionary<string, object>()
                        { { "Command", request.Command } }
                    );
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