namespace EventHorizon.Zone.System.Admin.ExternalHub;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Wizard.Events.Query;
using EventHorizon.Zone.System.Wizard.Events.Run;
using EventHorizon.Zone.System.Wizard.Model;

using global::System.Collections.Generic;
using global::System.Threading.Tasks;

/// <summary>
/// Make sure this stays on the ExternalHub root namespace.
/// This Class is encapsulating the Zone Info related logic,
///  and allows for a single SignalR hub to host all APIs.
/// </summary>
public partial class AdminHub
{
    public Task<CommandResult<IEnumerable<WizardMetadata>>> Wizard_All() => _mediator.Send(
        new QueryForAllWizards(),
        Context.ConnectionAborted
    );

    public Task<CommandResult<WizardData>> Wizard_RunScriptProcessor(
        string wizardId,
        string wizardStepId,
        string processorScriptId,
        WizardData wizardData
    ) => _mediator.Send(
        new RunWizardScriptProcessorCommand(
            wizardId,
            wizardStepId,
            processorScriptId,
            wizardData
        ),
        Context.ConnectionAborted
    );
}
