using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Server.Scripts.Model;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Script.Run
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
            readonly ActorBehaviorScriptRepository _scriptRepository;
            readonly ServerScriptServices _scriptServices;
            public RunBehaviorScriptHandler(
                ActorBehaviorScriptRepository scriptRepository,
                ServerScriptServices scriptServices
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