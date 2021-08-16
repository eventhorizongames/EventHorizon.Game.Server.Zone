using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.System.Interaction.Script.Api;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;

using MediatR;

namespace EventHorizon.Zone.System.Interaction.Script.Run
{
    public class RunInteractionScriptCommandHandler
        : IRequestHandler<RunInteractionScriptCommand, RunInteractionScriptResponse>
    {
        readonly IMediator _mediator;
        public RunInteractionScriptCommandHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        public async Task<RunInteractionScriptResponse> Handle(
            RunInteractionScriptCommand request,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new RunServerScriptCommand(
                    request.Interaction.ScriptId,
                    new Dictionary<string, object>()
                    {
                        { "Interaction", request.Interaction },
                        { "Player", request.Player },
                        { "Target", request.InteractionEntity },
                    }
                )
            );
            return new InternalRunInteractionScriptResponse();
        }
        public struct InternalRunInteractionScriptResponse : RunInteractionScriptResponse
        {
        }
    }
}
