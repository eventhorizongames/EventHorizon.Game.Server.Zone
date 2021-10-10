namespace EventHorizon.Zone.System.Player.Api
{
    using EventHorizon.Zone.Core.Model.Entity;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    public interface PlayerConfigurationState
        : PlayerConfigurationCache
    {
        Task<(bool Updated, ObjectEntityConfiguration OldConfig)> Set(
            ObjectEntityConfiguration playerConfiguration,
            CancellationToken cancellation
        );
    }
}
