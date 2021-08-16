using EventHorizon.Zone.Core.Model.Client;

namespace EventHorizon.Zone.System.Combat.Model.Client.Messsage
{
    public struct MessageFromCombatSystemData : IClientActionData
    {
        public string MessageCode { get; set; }
        public string Message { get; set; }
    }
}
