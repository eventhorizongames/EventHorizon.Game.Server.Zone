namespace EventHorizon.Zone.System.Agent.PopulateData
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Events.PopulateData;
    using EventHorizon.Zone.System.Agent.Model.Path;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class PopulateAgentPathDataHandler : INotificationHandler<PopulateAgentEntityDataEvent>
    {
        public Task Handle(
            PopulateAgentEntityDataEvent request,
            CancellationToken cancellationToken
        )
        {
            var agent = request.Agent;

            // Populate the Agent with a New Path State.
            agent.SetProperty(
                PathState.PROPERTY_NAME,
                PathState.NEW
            );

            return Task.CompletedTask;
        }
    }
}
