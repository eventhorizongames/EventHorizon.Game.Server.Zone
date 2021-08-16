namespace EventHorizon.Zone.System.Combat.Events.Client.Messsage
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.System.Combat.Model.Client.Messsage;

    public static class ClientActionMessageFromCombatSystemEvent
    {
        public static ClientActionGenericToAllEvent Create(
            MessageFromCombatSystemData data
        ) => new ClientActionGenericToAllEvent(
            "MessageFromCombatSystem",
            data
        );
    }
}
