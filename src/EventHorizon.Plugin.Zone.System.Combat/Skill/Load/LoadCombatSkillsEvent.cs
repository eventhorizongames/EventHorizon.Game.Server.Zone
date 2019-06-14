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
    public struct LoadCombatSkillsEvent : INotification
    {
        public struct LoadCombatSkillsHandler : INotificationHandler<LoadCombatSkillsEvent>
        {
            readonly IMediator _mediator;
            readonly ServerInfo _serverInfo;
            readonly IJsonFileLoader _fileLoader;
            readonly ISkillRepository _skillRepository;

            public LoadCombatSkillsHandler(
                IMediator mediator,
                ServerInfo serverInfo,
                IJsonFileLoader fileLoader,
                ISkillRepository skillRepository
            )
            {
                _mediator = mediator;
                _serverInfo = serverInfo;
                _fileLoader = fileLoader;
                _skillRepository = skillRepository;
            }

            public async Task Handle(
                LoadCombatSkillsEvent notification,
                CancellationToken cancellationToken
            )
            {
                var skillPath = Path.Combine(
                    _serverInfo.ClientPath,
                    "Skills"
                );
                var skillDirectory = new DirectoryInfo(
                    skillPath
                );

                foreach (var skillFile in skillDirectory.GetFiles())
                {
                    _skillRepository.Add(
                        await _fileLoader.GetFile<SkillInstance>(
                            skillFile.FullName
                        )
                    );
                }
            }
        }
    }
}