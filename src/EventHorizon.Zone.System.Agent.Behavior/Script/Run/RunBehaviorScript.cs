using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.Agent.Ai.Script;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Behavior.Script.Run
{
    public struct RunBehaviorScript : IRequest<BehaviorScriptResponse>
    {
        public IObjectEntity Actor { get; }
        public string ScriptId { get; }

        public RunBehaviorScript(
            IObjectEntity actor,
            string scriptId
        )
        {
            Actor = actor;
            ScriptId = scriptId;
        }

        public struct RunBehaviorScriptHandler : IRequestHandler<RunBehaviorScript, BehaviorScriptResponse>
        {
            readonly AgentBehaviorScriptRepository _scriptRepository;
            readonly IScriptServices _scriptServices;
            public RunBehaviorScriptHandler(
                AgentBehaviorScriptRepository scriptRepository,
                IScriptServices scriptServices
            )
            {
                _scriptRepository = scriptRepository;
                _scriptServices = scriptServices;
            }

            public Task<BehaviorScriptResponse> Handle(
                RunBehaviorScript request,
                CancellationToken cancellationToken
            ) => _scriptRepository.Find(
                    request.ScriptId
                ).Run(
                    _scriptServices,
                    request.Actor
                );
        }
    }
}