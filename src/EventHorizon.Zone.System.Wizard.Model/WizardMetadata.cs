namespace EventHorizon.Zone.System.Wizard.Model
{
    using global::System.Collections.Generic;

    public class WizardMetadata
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FirstStep { get; set; } = string.Empty;
        public List<WizardStep> StepList { get; set; } = new List<WizardStep>();
    }
}
