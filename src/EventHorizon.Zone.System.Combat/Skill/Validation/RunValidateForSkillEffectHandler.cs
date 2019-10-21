using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Combat.Skill.Model;
using EventHorizon.Zone.System.Combat.Script;
using MediatR;
using EventHorizon.Zone.System.Server.Scripts.Events.Run;

namespace EventHorizon.Zone.System.Combat.Skill.Validation
{
    public class RunValidateForSkillEffectHandler : IRequestHandler<RunValidateForSkillEffectEvent, IEnumerable<SkillValidatorResponse>>
    {
        readonly IMediator _mediator;
        readonly IScriptServices _scriptServices;

        public RunValidateForSkillEffectHandler(
            IMediator mediator,
            IScriptServices scriptServices
        )
        {
            _mediator = mediator;
            _scriptServices = scriptServices;
        }
        
        public async Task<IEnumerable<SkillValidatorResponse>> Handle(
            RunValidateForSkillEffectEvent request,
            CancellationToken cancellationToken
        )
        {
            var response = new List<SkillValidatorResponse>();
            foreach (var validator in request.SkillEffect.ValidatorList ?? new SkillValidator[0])
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
}