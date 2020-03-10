namespace EventHorizon.Zone.System.Agent.Plugin.Wild.PopulateData
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Agent.Plugin.Wild.Model;
    using EventHorizon.Zone.Core.Model.Entity;
    using MediatR;
    using EventHorizon.Zone.System.Agent.Events.PopulateData;

    public class PopulateAgentWildDataHandler : INotificationHandler<PopulateAgentEntityDataEvent>
    {
        public Task Handle(
            PopulateAgentEntityDataEvent request,
            CancellationToken cancellationToken
        )
        {
            var agent = request.Agent;

            // Populate the Agent's Wild State from Data.
            agent.PopulateData<AgentWildState>(
                AgentWildState.PROPERTY_NAME,
                AgentWildState.NEW
            );

            return Task.CompletedTask;
        }
    }
}