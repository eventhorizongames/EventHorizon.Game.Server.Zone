namespace EventHorizon.Zone.System.Wizard.Events.Json.Merge;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Wizard.Model;

using MediatR;

public struct MergeWizardDataIntoJsonCommand
    : IRequest<CommandResult<string>>
{
    public WizardData WizardData { get; }
    public string SourceJson { get; }

    public MergeWizardDataIntoJsonCommand(
        WizardData wizardData,
        string sourceJson
    )
    {
        WizardData = wizardData;
        SourceJson = sourceJson;
    }
}
