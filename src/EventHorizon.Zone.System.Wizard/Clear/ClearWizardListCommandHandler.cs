namespace EventHorizon.Zone.System.Wizard.Clear;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Wizard.Api;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class ClearWizardListCommandHandler
    : IRequestHandler<ClearWizardListCommand, StandardCommandResult>
{
    private readonly WizardRepository _wizardRepository;

    public ClearWizardListCommandHandler(
        WizardRepository wizardRepository
    )
    {
        _wizardRepository = wizardRepository;
    }

    public Task<StandardCommandResult> Handle(
        ClearWizardListCommand request,
        CancellationToken cancellationToken
    )
    {
        _wizardRepository.Clear();

        return new StandardCommandResult()
            .FromResult();
    }
}
