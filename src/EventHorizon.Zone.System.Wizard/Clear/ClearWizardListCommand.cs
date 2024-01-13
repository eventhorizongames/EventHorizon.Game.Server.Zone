namespace EventHorizon.Zone.System.Wizard.Clear;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public struct ClearWizardListCommand
    : IRequest<StandardCommandResult>
{
}
