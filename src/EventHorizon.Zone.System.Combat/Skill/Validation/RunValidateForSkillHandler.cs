using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Combat.Skill.Model;
using EventHorizon.Zone.System.Combat.Script;
using MediatR;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;

namespace EventHorizon.Zone.System.Combat.Skill.Validation
{
    public class RunValidateForSkillHandler : IRequestHandler<RunValidateForSkillEvent, SkillValidatorResponse>
    {
        readonly IMediator _mediator;
        readonly IScriptServices _scriptServices;
        public RunValidateForSkillHandler(
            IMediator mediator,
            IScriptServices scriptServices
        )
        {
            _mediator = mediator;
            _scriptServices = scriptServices;
        }
        public async Task<SkillValidatorResponse> Handle(
            RunValidateForSkillEvent request,
            CancellationToken cancellationToken
        )
        {
            foreach (var validator in request.Skill.ValidatorList)
            {
                var response = (SkillValidatorResponse)await _mediator.Send(
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

                if (!response.Success)
                {
                    return response;
                }
            }

            return new SkillValidatorResponse
            {
                Success = true
            };
        }
    }
}