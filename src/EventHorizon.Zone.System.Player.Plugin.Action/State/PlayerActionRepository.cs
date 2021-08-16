using System.Collections.Generic;

using EventHorizon.Zone.System.Player.Plugin.Action.Model;

namespace EventHorizon.Zone.System.Player.Plugin.Action.State
{
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
