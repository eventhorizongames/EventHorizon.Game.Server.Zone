namespace EventHorizon.Zone.System.Player.Plugin.Action.State
{
    using EventHorizon.Zone.System.Player.Plugin.Action.Model;

    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Linq;

    public class InMemoryPlayerActionRepository
        : PlayerActionRepository
    {
        private readonly ConcurrentDictionary<long, PlayerActionEntity> _map = new();

        public void On(
            PlayerActionEntity action
        )
        {
            if (_map.ContainsKey(
                action.Id
            ))
            {
                throw new AlreadyContainsPlayerAction(
                    action.Id
                );
            }
            _map.TryAdd(
                action.Id,
                action
            );
        }

        public IEnumerable<PlayerActionEntity> Where(
            string actionName
        )
        {
            return _map.Where(
                a => a.Value.ActionName == actionName
            ).Select(
                pair => pair.Value
            );
        }
    }
}
