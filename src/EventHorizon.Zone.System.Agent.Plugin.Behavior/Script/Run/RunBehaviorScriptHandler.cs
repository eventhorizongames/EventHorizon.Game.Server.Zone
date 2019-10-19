using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Script.Run
{
    public struct RunBehaviorScriptHandler : IRequestHandler<RunBehaviorScript, BehaviorScriptResponse>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;

        public RunBehaviorScriptHandler(
            ILogger<RunBehaviorScriptHandler> logger,
            IMediator mediator
        )
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<BehaviorScriptResponse> Handle(
            RunBehaviorScript request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                return (BehaviorScriptResponse)await _mediator.Send(
                    new RunServerScriptCommand(
                        request.ScriptId,
                        new Dictionary<string, object>()
                        {
                            { "Actor", request.Actor },
                        }
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Problem Running Behavior Script",
                    request
                );
                return new BehaviorScriptResponse(
                    BehaviorNodeStatus.ERROR
                );
            }
        }
    }
}