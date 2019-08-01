using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Plugin.Interaction.Script.Api;
using EventHorizon.Zone.Plugin.Interaction.Script.State;
using MediatR;

namespace EventHorizon.Zone.Plugin.Interaction.Script.Run
{
    public struct RunInteractionScriptCommandHandler
        : IRequestHandler<RunInteractionScriptCommand, RunInteractionScriptResponse>
    {
        readonly InteractionScriptRepository _repository;
        public RunInteractionScriptCommandHandler(
            InteractionScriptRepository repository
        )
        {
            _repository = repository;
        }
        public async Task<RunInteractionScriptResponse> Handle(
            RunInteractionScriptCommand request,
            CancellationToken cancellationToken
        )
        {
            var script = _repository.Get(
                request.Interaction.ScriptId
            );
            await script.Run(
                request.Player,
                request.InteractionEntity,
                request.Interaction.Data
            );
            return new InternalRunInteractionScriptResponse();
        }
        public struct InternalRunInteractionScriptResponse : RunInteractionScriptResponse
        {
        }
    }
}