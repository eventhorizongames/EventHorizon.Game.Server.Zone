namespace EventHorizon.Zone.System.Wizard.Run
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Server.Scripts.Events.Run;
    using EventHorizon.Zone.System.Wizard.Api;
    using EventHorizon.Zone.System.Wizard.Events.Run;
    using EventHorizon.Zone.System.Wizard.Model;
    using EventHorizon.Zone.System.Wizard.Model.Scripts;

    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class RunWizardScriptProcessorCommandHandler
        : IRequestHandler<RunWizardScriptProcessorCommand, CommandResult<WizardData>>
    {
        private readonly IMediator _mediator;
        private readonly WizardRepository _wizardRepository;

        public RunWizardScriptProcessorCommandHandler(
            IMediator mediator,
            WizardRepository wizardRepository
        )
        {
            _mediator = mediator;
            _wizardRepository = wizardRepository;
        }

        public async Task<CommandResult<WizardData>> Handle(
            RunWizardScriptProcessorCommand request,
            CancellationToken cancellationToken
        )
        {
            var wizardData = request.WizardData;
            var wizardOption = _wizardRepository.Get(
                request.WizardId
            );
            if (!wizardOption.HasValue)
            {
                return "wizard_not_found";
            }

            var wizardStep = wizardOption.Value.StepList.FirstOrDefault(
                a => a.Id == request.WizardStepId
            );
            if (wizardStep.IsNull())
            {
                return "wizard_step_not_found";
            }

            var processorScriptId = string.Empty;
            if (IsInvalidProcessorId(
                wizardStep,
                request.ProcessorScriptId,
                out processorScriptId
            ))
            {
                return "wizard_invalid_processor_script_id";
            }

            var scriptData = new Dictionary<string, object>
            {
                ["Wizard"] = wizardOption.Value,
                ["WizardStep"] = wizardStep,
                ["WizardData"] = wizardData,
            };

            var result = await _mediator.Send(
                new RunServerScriptCommand(
                    processorScriptId,
                    scriptData
                ),
                cancellationToken
            );

            if (result.IsNull()
                || !result.Success
            )
            {
                return result?.Message
                    ?? "wizard_failed_script_run";
            }

            if (result is WizardServerScriptResponse wizardScriptResponse)
            {
                return new WizardData(
                    wizardScriptResponse.Data
                );
            }

            return "wizard_failed_script_run";
        }

        private static bool IsInvalidProcessorId(
            WizardStep wizardStep,
            string expectedProcessorScriptId,
            out string processorScriptId
        ) => !wizardStep.Details.TryGetValue(
            "Processor:ScriptId",
            out processorScriptId
        ) || string.IsNullOrWhiteSpace(
            processorScriptId
        ) || processorScriptId != expectedProcessorScriptId;
    }
}
