using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Client;

namespace EventHorizon.Plugin.Zone.System.Combat.Events.Client.Messsage
{
    public struct MessageFromCombatSystemData : IClientActionData
    {
        public string MessageCode { get; set; }
        public string Message { get; set; }
    }
}