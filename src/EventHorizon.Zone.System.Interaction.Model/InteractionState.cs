using System.Collections.Generic;

namespace EventHorizon.Zone.System.Interaction.Model
{
    public struct InteractionState
    {
        public static readonly string PROPERTY_NAME = "interactionState";

        public bool Active { get; set; }
        public IList<InteractionItem> List { get; set; }

        public static readonly InteractionState NEW = new InteractionState
        {
            Active = true,

            List = null
        };
    }
}