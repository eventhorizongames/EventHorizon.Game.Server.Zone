namespace EventHorizon.Zone.System.Interaction.Model
{
    using global::System.Collections.Generic;

    public class InteractionState
    {
        public static readonly string PROPERTY_NAME = "interactionState";

        public bool Active { get; set; } = true;
        public int DistanceToPlayer { get; set; } = 10;
        public string ParticleTemplate { get; set; } = string.Empty;
        public IList<InteractionItem>? List { get; set; }

        public static readonly InteractionState NEW = new()
        {
            Active = true,
            DistanceToPlayer = 10,
            ParticleTemplate = string.Empty,
            List = null,
        };
    }
}
