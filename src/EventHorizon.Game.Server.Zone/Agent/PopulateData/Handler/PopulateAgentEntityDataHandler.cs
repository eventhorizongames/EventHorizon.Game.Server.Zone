using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.PopulateData.Handler
{
    public class PopulateAgentEntityDataHandler : IRequestHandler<PopulateAgentEntityDataEvent, AgentEntity>
    {
        public Task<AgentEntity> Handle(PopulateAgentEntityDataEvent request, CancellationToken cancellationToken)
        {
            var agent = request.Agent;

            agent.PopulateFromTempData<AiRoutine>("Routine");
            agent.PopulateFromTempData<AgentAiState>("Ai");

            return Task.FromResult(agent);
        }
    }
}