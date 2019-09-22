using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Combat.Plugin.Editor.Skills.Model;
using EventHorizon.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Plugin.Editor.Skills.Query
{
    public struct EditorSystemCombatSkillScriptsFileQueryHandler : IRequestHandler<EditorSystemCombatSkillScriptsFileQuery, EditorSystemCombatSkillScriptsFile>
    {
        readonly ISkillEffectScriptRepository _effectScriptRepository;
        readonly ISkillValidatorScriptRepository _validatorScriptRepository;

        public EditorSystemCombatSkillScriptsFileQueryHandler(
            ISkillEffectScriptRepository effectScriptRepository,
            ISkillValidatorScriptRepository validatorScriptRepository
        )
        {
            _effectScriptRepository = effectScriptRepository;
            _validatorScriptRepository = validatorScriptRepository;
        }

        public Task<EditorSystemCombatSkillScriptsFile> Handle(
            EditorSystemCombatSkillScriptsFileQuery request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                new EditorSystemCombatSkillScriptsFile(
                    _effectScriptRepository.All(),
                    _validatorScriptRepository.All()
                )
            );
        }
    }
}