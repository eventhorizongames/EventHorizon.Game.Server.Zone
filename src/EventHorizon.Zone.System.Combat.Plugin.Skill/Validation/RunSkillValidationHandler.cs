namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Validation;

using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class RunSkillValidationHandler
    : IRequestHandler<RunSkillValidation, IEnumerable<SkillValidatorResponse>>
{
    private readonly IMediator _mediator;

    public RunSkillValidationHandler(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public async Task<IEnumerable<SkillValidatorResponse>> Handle(
        RunSkillValidation request,
        CancellationToken cancellationToken
    )
    {
        var response = new List<SkillValidatorResponse>();
        foreach (var validator in request.ValidatorList)
        {
            var scriptResponse = (SkillValidatorResponse)await _mediator.Send(
                new RunServerScriptCommand(
                    validator.Validator,
                    new Dictionary<string, object>()
                    {
                        { "Caster", request.Caster },
                        { "Target", request.Target },
                        { "Skill", request.Skill },
                        { "TargetPosition", request.TargetPosition },
                        { "ValidatorData", validator.Data },
                    }
                )
            );
            response.Add(
                scriptResponse
            );
            if (!scriptResponse.Success)
            {
                break;
            }
        }
        return response;
    }
}
