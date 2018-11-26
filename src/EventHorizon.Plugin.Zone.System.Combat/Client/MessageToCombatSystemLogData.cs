using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Client;

namespace EventHorizon.Plugin.Zone.System.Combat.Client
{
    public struct MessageToCombatSystemLogData : IClientActionData
    {
        public string MessageCode { get; set; }
        public string Message { get; set; }
    }
}