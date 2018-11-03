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
    public class LoadCombatSkillsHandler : INotificationHandler<LoadCombatSkillsEvent>
    {
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;
        readonly ISkillRepository _skillRepository;

        public LoadCombatSkillsHandler(
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            ISkillRepository skillRepository
        )
        {
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
            _skillRepository = skillRepository;
        }

        public async Task Handle(LoadCombatSkillsEvent notification, CancellationToken cancellationToken)
        {

            var filePath = Path.Combine(_serverInfo.PluginsPath, "Combat.Skills.json");
            var combatSkillsFile = await _fileLoader.GetFile<CombatSkillsFile>(filePath);
            foreach (var skill in combatSkillsFile.SkillList)
            {
                _skillRepository.Add(
                    skill
                );
            }
        }
        private struct CombatSkillsFile
        {
            public List<SkillInstance> SkillList { get; set; }
        }
    }

}