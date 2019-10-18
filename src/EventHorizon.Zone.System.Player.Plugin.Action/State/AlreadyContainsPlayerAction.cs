using System;

namespace EventHorizon.Zone.System.Player.Plugin.Action.State
{
    [Serializable]
    public class AlreadyContainsPlayerAction : Exception
    {
        public long ActionId { get; } = -1;

        public AlreadyContainsPlayerAction(
            long actionId
        ) : base(
            "Please remove Player Action before adding another of the same Player Action Id."
        )
        {
            ActionId = actionId;
        }
    }
}