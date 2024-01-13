namespace EventHorizon.Zone.System.Wizard.Model;

public class WizardStep
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public WizardStepDetails Details { get; set; } = new WizardStepDetails();
    public string? NextStep { get; set; }
    public string? PreviousStep { get; set; }
}
