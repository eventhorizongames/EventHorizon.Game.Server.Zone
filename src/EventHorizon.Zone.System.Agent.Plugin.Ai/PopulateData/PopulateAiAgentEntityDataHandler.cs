using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Plugin.Ai.Model;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;
using EventHorizon.Zone.System.Agent.Events.PopulateData;

namespace EventHorizon.Zone.System.Agent.Plugin.Ai.PopulateData
{
    public class PopulateAgentEntityDataHandler : INotificationHandler<PopulateAgentEntityDataEvent>
    {
        public Task Handle(
            PopulateAgentEntityDataEvent request,
            CancellationToken cancellationToken
        )
        {
            var agent = request.Agent;

            // Move these to a Repository, and have them populated from plugins.
            agent.PopulateData<AgentWanderState>(
                AgentWanderState.WANDER_NAME
            );

            return Task.CompletedTask;
        }
    }
}