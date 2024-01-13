namespace EventHorizon.Zone.System.Wizard.Json.Merge;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Wizard.Events.Json.Merge;
using EventHorizon.Zone.System.Wizard.Json.Mapper;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class MergeWizardDataIntoJsonCommandHandler
    : IRequestHandler<MergeWizardDataIntoJsonCommand, CommandResult<string>>
{
    private readonly IMediator _mediator;

    public MergeWizardDataIntoJsonCommandHandler(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public async Task<CommandResult<string>> Handle(
        MergeWizardDataIntoJsonCommand request,
        CancellationToken cancellationToken
    )
    {
        var wizardDataAsJsonDocument = await _mediator.Send(
            new MapWizardDataToJsonDataDocument(
                request.WizardData
            ),
            cancellationToken
        );

        var jsonMergeResult = await _mediator.Send(
            new MergeJsonStringsIntoSingleJsonStringCommand(
                request.SourceJson,
                wizardDataAsJsonDocument.ToJsonString()
            ),
            cancellationToken
        );

        if (!jsonMergeResult.Success)
        {
            return new(
                jsonMergeResult.ErrorCode
            );
        }

        return new(
            true,
            jsonMergeResult.Result
        );
    }
}
