using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.PopulateData.Handler
{
    public class PopulateBaseAgentEntityDataHandler : INotificationHandler<PopulateAgentEntityDataEvent>
    {
        public Task Handle(PopulateAgentEntityDataEvent request, CancellationToken cancellationToken)
        {
            var agent = request.Agent;

            // Move these to a Repository, and have them populated from plugins.
            agent.PopulateData<AgentRoutine>("Routine");
            agent.PopulateData<AgentRoutine>("DefaultRoutine");

            return Task.CompletedTask;
        }
    }
}