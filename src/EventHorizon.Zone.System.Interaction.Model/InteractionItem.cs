namespace EventHorizon.Zone.System.Interaction.Model;

using global::System.Collections;
using global::System.Collections.Generic;

public struct InteractionItem
{
    public string ScriptId { get; set; }
    public int DistanceToPlayer { get; set; }
    public IDictionary<string, object> Data { get; set; }
}
