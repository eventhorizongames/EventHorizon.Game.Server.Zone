namespace EventHorizon.Zone.System.Wizard.Run
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Wizard.Events.Run;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class RunWizardScriptProcessorCommandHandler
        : IRequestHandler<RunWizardScriptProcessorCommand, StandardCommandResult>
    {
        public async Task<StandardCommandResult> Handle(
            RunWizardScriptProcessorCommand request,
            CancellationToken cancellationToken
        )
        {
            // TODO: Finish Implementation

            // TODO: Get Wizard
            // TODO: Get Wizard Step from Wizard
            var wizardData = request.WizardData;
            // TODO: Create Script Data Dictionary

            // TODO: Run Script found in Wizard Step

            await Task.Delay(2000);

            return new(
                "NOT_IMPLEMENTED"
            );
        }
    }
}
