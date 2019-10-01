using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;
using EventHorizon.Zone.System.Agent.Events.PopulateData;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.PopulateData
{
    public class PopulateBaseAgentEntityDataHandler : INotificationHandler<PopulateAgentEntityDataEvent>
    {
        public Task Handle(
            PopulateAgentEntityDataEvent request,
            CancellationToken cancellationToken
        )
        {
            var agent = request.Agent;

            agent.PopulateData<AgentBehavior>(
                AgentBehavior.PROPERTY_NAME
            );
            var behavior = agent.GetProperty<AgentBehavior>(
                AgentBehavior.PROPERTY_NAME
            );
            if (behavior.TreeId == null)
            {
                agent.SetProperty(
                    AgentBehavior.PROPERTY_NAME,
                    AgentBehavior.NEW
                );
            }

            return Task.CompletedTask;
        }
    }
}