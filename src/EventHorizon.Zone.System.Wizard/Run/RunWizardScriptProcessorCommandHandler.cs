namespace EventHorizon.Zone.System.Wizard.Run
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Server.Scripts.Events.Run;
    using EventHorizon.Zone.System.Wizard.Api;
    using EventHorizon.Zone.System.Wizard.Events.Run;
    using EventHorizon.Zone.System.Wizard.Model;
    using EventHorizon.Zone.System.Wizard.Model.Scripts;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class RunWizardScriptProcessorCommandHandler
        : IRequestHandler<RunWizardScriptProcessorCommand, CommandResult<WizardData>>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly WizardRepository _wizardRepository;

        public RunWizardScriptProcessorCommandHandler(
            ILogger<RunWizardScriptProcessorCommandHandler> logger,
            IMediator mediator,
            WizardRepository wizardRepository
        )
        {
            _logger = logger;
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
                return WizardScriptProcessorErrorCodes.WIZARD_NOT_FOUND;
            }

            var wizardStep = wizardOption.Value.StepList.FirstOrDefault(
                a => a.Id == request.WizardStepId
            );
            if (wizardStep.IsNull())
            {
                return WizardScriptProcessorErrorCodes.WIZARD_STEP_NOT_FOUND;
            }

            var processorScriptId = string.Empty;
            if (IsInvalidProcessorId(
                wizardStep,
                request.ProcessorScriptId,
                out processorScriptId
            ))
            {
                return WizardScriptProcessorErrorCodes.WIZARD_INVALID_PROCESSOR_SCRIPT_ID;
            }

            var scriptData = new Dictionary<string, object>
            {
                ["Wizard"] = wizardOption.Value,
                ["WizardStep"] = wizardStep,
                ["WizardData"] = wizardData,
            };

            try
            {
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
                        ?? WizardScriptProcessorErrorCodes.WIZARD_FAILED_SCRIPT_RUN;
                }

                if (result is WizardServerScriptResponse wizardScriptResponse)
                {
                    return new WizardData(
                        wizardScriptResponse.Data
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to Run Server Script Command: {ProcessorScriptId} | {@ScriptData}",
                    processorScriptId,
                    scriptData
                );

                return WizardScriptProcessorErrorCodes.WIZARD_FAILED_SCRIPT_RUN;
            }

            return WizardScriptProcessorErrorCodes.WIZARD_FAILED_SCRIPT_RUN;
        }

        private static bool IsInvalidProcessorId(
            WizardStep wizardStep,
            string expectedProcessorScriptId,
            [MaybeNullWhen(true)] out string processorScriptId
        ) => !wizardStep.Details.TryGetValue(
            "Processor:ScriptId",
            out processorScriptId
        ) || string.IsNullOrWhiteSpace(
            processorScriptId
        ) || processorScriptId != expectedProcessorScriptId;
    }
}
