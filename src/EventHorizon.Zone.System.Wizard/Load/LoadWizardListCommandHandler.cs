namespace EventHorizon.Zone.System.Wizard.Load
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Wizard.Api;
    using EventHorizon.Zone.System.Wizard.Model;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class LoadWizardListCommandHandler
        : IRequestHandler<LoadWizardListCommand, StandardCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly IJsonFileLoader _jsonFileLoader;
        private readonly WizardRepository _wizardRepository;

        public LoadWizardListCommandHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader jsonFileLoader,
            WizardRepository wizardRepository
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _jsonFileLoader = jsonFileLoader;
            _wizardRepository = wizardRepository;
        }

        public async Task<StandardCommandResult> Handle(
            LoadWizardListCommand request,
            CancellationToken cancellationToken
        )
        {
            _wizardRepository.Clear();

            await _mediator.Send(
                new ProcessFilesRecursivelyFromDirectory(
                    Path.Combine(
                        _serverInfo.ServerPath,
                        "Wizards"
                    ),
                    OnProcessFile,
                    new Dictionary<string, object>()
                ),
                cancellationToken
            );

            return new();
        }

        private async Task OnProcessFile(
            StandardFileInfo fileInfo,
            IDictionary<string, object> arguments
        )
        {
            var wizard = await _jsonFileLoader.GetFile<WizardMetadata>(
                fileInfo.FullName
            );

            if (string.IsNullOrWhiteSpace(
                wizard.Id
            ))
            {
                return;
            }

            _wizardRepository.Set(
                wizard.Id,
                wizard
            );
        }
    }
}
