namespace EventHorizon.Zone.System.Selection.Model
{
    public class SelectionState
    {
        public static readonly string PROPERTY_NAME = "selectionState";

        public string SelectedCompanionParticleTemplate { get; set; } = string.Empty;
        public string SelectedParticleTemplate { get; set; } = string.Empty;

        public static readonly SelectionState NEW = new()
        {
            SelectedCompanionParticleTemplate = string.Empty,
            SelectedParticleTemplate = string.Empty,
        };
    }
}
