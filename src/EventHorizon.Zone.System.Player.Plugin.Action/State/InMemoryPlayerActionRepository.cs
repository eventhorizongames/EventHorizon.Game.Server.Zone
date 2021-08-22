namespace EventHorizon.Zone.System.Player.Plugin.Action.State
{
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Linq;

    using EventHorizon.Zone.System.Player.Plugin.Action.Model;

    public class InMemoryPlayerActionRepository : PlayerActionRepository
    {
        private readonly ConcurrentDictionary<long, PlayerActionEntity> MAP = new ConcurrentDictionary<long, PlayerActionEntity>();

        public void On(
            PlayerActionEntity action
        )
        {
            if (MAP.ContainsKey(
                action.Id
            ))
            {
                throw new AlreadyContainsPlayerAction(
                    action.Id
                );
            }
            MAP.TryAdd(
                action.Id,
                action
            );
        }

        public IEnumerable<PlayerActionEntity> Where(
            string actionName
        )
        {
            return MAP.Where(
                a => a.Value.ActionName == actionName
            ).Select(
                pair => pair.Value
            );
        }
    }
}
