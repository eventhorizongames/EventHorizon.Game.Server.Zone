namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Script.Run;

using global::System;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;
using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

public class RunBehaviorScriptHandler
    : IRequestHandler<RunBehaviorScript, BehaviorScriptResponse>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

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
            if (request.ScriptId == "$DEFAULT$SCRIPT")
            {
                return new BehaviorScriptResponse(BehaviorNodeStatus.SUCCESS);
            }
            var result = await _mediator.Send(
                new RunServerScriptCommand(
                    request.ScriptId,
                    new Dictionary<string, object>() { { "Actor", request.Actor }, }
                )
            );
            if (result is BehaviorScriptResponse response)
            {
                return response;
            }
            _logger.LogError(
                "Behavior Script result was not expected type:  \n | request.ScriptId: {RequestScriptId} \n | result.Message: {ResultMessage}",
                request.ScriptId,
                result.Message
            );
            return new BehaviorScriptResponse(BehaviorNodeStatus.FAILED);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Problem Running Behavior Script. \n | request.ScriptId: {RequestScriptId}",
                request.ScriptId
            );
            return new BehaviorScriptResponse(BehaviorNodeStatus.ERROR);
        }
    }
}
