namespace EventHorizon.Zone.System.Wizard.Load
{
    using EventHorizon.Zone.Core.Model.Command;

    using MediatR;

    public struct LoadWizardListCommand
        : IRequest<StandardCommandResult>
    {
    }
}
