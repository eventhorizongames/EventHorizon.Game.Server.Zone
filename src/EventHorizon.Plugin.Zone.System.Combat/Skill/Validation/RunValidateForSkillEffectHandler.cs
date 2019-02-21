
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Script;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Validation
{
    public class RunValidateForSkillEffectHandler : IRequestHandler<RunValidateForSkillEffectEvent, IEnumerable<SkillValidatorResponse>>
    {
        readonly ISkillValidatorScriptRepository _validatorScriptRepository;
        readonly IScriptServices _scriptServices;
        public RunValidateForSkillEffectHandler(
            ISkillValidatorScriptRepository validatorScriptRepository,
            IScriptServices scriptServices
        )
        {
            _validatorScriptRepository = validatorScriptRepository;
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
                var script = _validatorScriptRepository.Find(
                    validator.Validator
                );
                var scriptResponse = await script.Run(
                    _scriptServices,
                    request.Caster,
                    request.Target,
                    request.Skill,
                    request.TargetPosition,
                    validator.Data
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