namespace EventHorizon.Zone.System.Combat.Events.Client.Messsage
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.System.Combat.Model.Client.Messsage;

    public static class SingleClientActionMessageFromCombatSystemEvent
    {
        public static ClientActionGenericToSingleEvent Create(
            string connectionId,
            MessageFromCombatSystemData data
        ) => new ClientActionGenericToSingleEvent(
            connectionId,
            "MessageFromCombatSystem",
            data
        );
    }
}