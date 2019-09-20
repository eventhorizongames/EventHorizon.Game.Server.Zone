using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Companion.Model;
using EventHorizon.Game.Server.Zone.Agent.PopulateData;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Companion.PopulateData
{
    public class PopulateAgentCompanionDataHandler : INotificationHandler<PopulateAgentEntityDataEvent>
    {
        public Task Handle(
            PopulateAgentEntityDataEvent request,
            CancellationToken cancellationToken
        )
        {
            var agent = request.Agent;

            // Populates the Owner State on the Agent from loaded data.
            agent.PopulateData<OwnerState>(OwnerState.PROPERTY_NAME);

            return Task.CompletedTask;
        }
    }
}