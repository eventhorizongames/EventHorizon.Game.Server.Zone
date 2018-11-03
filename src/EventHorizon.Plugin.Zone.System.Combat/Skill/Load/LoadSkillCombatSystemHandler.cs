using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Load
{
    public class LoadSkillCombatSystemHandler : INotificationHandler<LoadSkillCombatSystemEvent>
    {
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;
        readonly ISkillEffectScriptRepository _skillEffectScriptRepository;
        readonly ISkillActionScriptRepository _skillActionScriptRepository;

        public LoadSkillCombatSystemHandler(
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            ISkillEffectScriptRepository skillEffectScriptRepository,
            ISkillActionScriptRepository skillActionScriptRepository
        )
        {
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
            _skillEffectScriptRepository = skillEffectScriptRepository;
            _skillActionScriptRepository = skillActionScriptRepository;
        }
        public async Task Handle(LoadSkillCombatSystemEvent notification, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(_serverInfo.PluginsPath, "Skill.System.Combat.json");
            var skillSystemCombatFile = await _fileLoader.GetFile<SkillSystemCombatFile>(filePath);
            var scriptEffectsPath = Path.Combine(_serverInfo.AssetsPath, "Scripts", "Effects");
            var scriptActionsPath = Path.Combine(_serverInfo.AssetsPath, "Scripts", "Actions");

            foreach (var effect in skillSystemCombatFile.EffectList)
            {
                effect.CreateScript(
                    scriptEffectsPath
                );
                _skillEffectScriptRepository.Add(
                    effect
                );
            }

            foreach (var action in skillSystemCombatFile.ActionList)
            {
                action.CreateScript(
                    scriptActionsPath
                );
                _skillActionScriptRepository.Add(
                    action
                );
            }
        }

        private struct SkillSystemCombatFile
        {
            public List<SkillEffectScript> EffectList { get; set; }
            public List<SkillActionScript> ActionList { get; set; }
        }
    }
}