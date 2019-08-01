using System.Collections;
using System.Collections.Generic;

namespace EventHorizon.Zone.Plugin.Interaction.Model
{
    public struct InteractionItem
    {
        public string ScriptId { get; set; }
        public int DistanceToPlayer { get; set; }
        public IDictionary<string, object> Data { get; set; }
    }
}