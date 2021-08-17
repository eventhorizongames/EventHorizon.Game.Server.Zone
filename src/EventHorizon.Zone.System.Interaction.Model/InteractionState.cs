namespace EventHorizon.Zone.System.Interaction.Model
{
    using global::System.Collections.Generic;

    public struct InteractionState
    {
        public static readonly string PROPERTY_NAME = "interactionState";

        public bool Active { get; set; }
        public IList<InteractionItem>? List { get; set; }

        public static readonly InteractionState NEW = new()
        {
            Active = true,

            List = null
        };
    }
}
