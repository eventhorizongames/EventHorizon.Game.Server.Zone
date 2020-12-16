namespace EventHorizon.Zone.System.Combat.Load
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class LoadCombatSystemHandler
        : INotificationHandler<LoadCombatSystemEvent>
    {
        public Task Handle(
            LoadCombatSystemEvent notification,
            CancellationToken cancellationToken
        )
        {
            return Task.CompletedTask;
        }
    }
}