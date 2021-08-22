namespace EventHorizon.Zone.System.Combat.Model.Client.Messsage
{
    using EventHorizon.Zone.Core.Model.Client;

    public struct MessageFromCombatSystemData : IClientActionData
    {
        public string MessageCode { get; set; }
        public string Message { get; set; }
    }
}
