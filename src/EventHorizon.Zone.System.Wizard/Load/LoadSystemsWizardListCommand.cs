namespace EventHorizon.Zone.System.Wizard.Load
{
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public struct LoadSystemsWizardListCommand
        : IRequest<StandardCommandResult>
    {
    }
}
