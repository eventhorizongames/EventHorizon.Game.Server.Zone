namespace EventHorizon.Zone.System.Wizard.Events.Query
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Wizard.Model;

    using global::System.Collections.Generic;

    using MediatR;

    public struct QueryForAllWizards
        : IRequest<CommandResult<IEnumerable<WizardMetadata>>>
    {
    }
}
