namespace EventHorizon.Zone.System.Player.Plugin.Action.State
{
    using global::System.Collections.Generic;

    using EventHorizon.Zone.System.Player.Plugin.Action.Model;

    public interface PlayerActionRepository
    {
        void On(
            PlayerActionEntity action
        );
        IEnumerable<PlayerActionEntity> Where(
            string actionName
        );
    }
}
