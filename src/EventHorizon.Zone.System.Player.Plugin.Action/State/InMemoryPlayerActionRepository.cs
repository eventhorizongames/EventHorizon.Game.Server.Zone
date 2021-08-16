using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using EventHorizon.Zone.System.Player.Plugin.Action.Model;

namespace EventHorizon.Zone.System.Player.Plugin.Action.State
{
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
