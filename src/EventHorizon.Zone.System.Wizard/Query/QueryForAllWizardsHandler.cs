namespace EventHorizon.Zone.System.Wizard.Query
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Wizard.Api;
    using EventHorizon.Zone.System.Wizard.Events.Query;
    using EventHorizon.Zone.System.Wizard.Model;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class QueryForAllWizardsHandler
        : IRequestHandler<QueryForAllWizards, CommandResult<IEnumerable<WizardMetadata>>>
    {
        private readonly WizardRepository _wizardRepository;

        public QueryForAllWizardsHandler(
            WizardRepository wizardRepository
        )
        {
            _wizardRepository = wizardRepository;
        }

        public Task<CommandResult<IEnumerable<WizardMetadata>>> Handle(
            QueryForAllWizards request,
            CancellationToken cancellationToken
        ) => new CommandResult<IEnumerable<WizardMetadata>>(
            _wizardRepository.All
        ).FromResult();
    }
}
