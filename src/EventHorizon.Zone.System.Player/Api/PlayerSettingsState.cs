namespace EventHorizon.Zone.System.Player.Api
{
    using EventHorizon.Zone.Core.Model.Entity;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    public interface PlayerSettingsState
        : PlayerSettingsCache
    {
        Task<(bool Updated, ObjectEntityConfiguration OldConfig)> SetConfiguration(
            ObjectEntityConfiguration playerConfiguration,
            CancellationToken cancellationToken
        );

        Task<(bool Updated, ObjectEntityData OldData)> SetData(
            ObjectEntityData playerData,
            CancellationToken cancellationToken
        );
    }
}
