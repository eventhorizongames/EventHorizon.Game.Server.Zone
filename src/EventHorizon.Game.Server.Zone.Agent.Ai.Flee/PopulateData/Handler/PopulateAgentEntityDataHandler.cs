using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Ai.Flee.Model.Ai;
using EventHorizon.Game.Server.Zone.Agent.PopulateData;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.Flee.PopulateData.Handler
{
    public class PopulateAgentEntityDataHandler : INotificationHandler<PopulateAgentEntityDataEvent>
    {
        public Task Handle(PopulateAgentEntityDataEvent request, CancellationToken cancellationToken)
        {
            var agent = request.Agent;

            agent.PopulateFromTempData<AgentFleeState>("Flee");

            return Task.CompletedTask;
        }
    }
}