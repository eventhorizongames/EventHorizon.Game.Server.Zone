namespace EventHorizon.Game.Server.Zone.Player.PopulateData
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Entity.Data;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;

    using MediatR;

    public class PopulatePlayerDefaultGameDataHandler
        : INotificationHandler<PopulateEntityDataEvent>
    {
        private static CompanionManagementState NEW_PLAYER_COMPANION_STATE = new()
        {
            CapturedBehaviorTreeId = "Behaviors_FollowOwner.json",
        };

        public Task Handle(
            PopulateEntityDataEvent request,
            CancellationToken cancellationToken
        )
        {
            var entity = request.Entity;

            if (entity.Type != EntityType.PLAYER)
            {
                return Task.CompletedTask;
            }

            // Populate the Companion Management State on the Player.
            entity.SetProperty(
                CompanionManagementState.PROPERTY_NAME,
                NEW_PLAYER_COMPANION_STATE
            );

            return Task.CompletedTask;
        }
    }
}
