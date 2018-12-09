using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Services;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Validation
{
    public class RunValidateForSkillHandler : IRequestHandler<RunValidateForSkillEvent, SkillValidatorResponse>
    {
        readonly ISkillValidatorScriptRepository _validatorScriptRepository;
        readonly IScriptServices _scriptServices;
        public RunValidateForSkillHandler(
            ISkillValidatorScriptRepository validatorScriptRepository,
            IScriptServices scriptServices
        )
        {
            _validatorScriptRepository = validatorScriptRepository;
            _scriptServices = scriptServices;
        }
        public async Task<SkillValidatorResponse> Handle(
            RunValidateForSkillEvent request,
            CancellationToken cancellationToken
        )
        {
            foreach (var validator in request.Skill.ValidatorList)
            {
                var script = _validatorScriptRepository.Find(
                    validator.Validator
                );
                var response = await script.Run(
                    _scriptServices,
                    request.Caster,
                    request.Target,
                    request.Skill,
                    validator.Data
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