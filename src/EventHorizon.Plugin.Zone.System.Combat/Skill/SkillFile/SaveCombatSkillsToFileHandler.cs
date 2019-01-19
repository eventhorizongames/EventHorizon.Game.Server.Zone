using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.SkillFile
{
    public struct SaveCombatSkillsToFileHandler : INotificationHandler<SaveCombatSkillsToFileEvent>
    {
        readonly ServerInfo _serverInfo;
        readonly IJsonFileSaver _fileSaver;
        public SaveCombatSkillsToFileHandler(
            ServerInfo serverInfo,
            IJsonFileSaver fileSaver
        )
        {
            _serverInfo = serverInfo;
            _fileSaver = fileSaver;
        }
        public Task Handle(SaveCombatSkillsToFileEvent notification, CancellationToken cancellationToken)
        {
            return _fileSaver.SaveToFile(
                _serverInfo.PluginsPath,
                "Combat.Skills.json",
                new CombatSkillsFile
                {
                    SkillList = notification.SkillList
                }
            );
        }
    }
}