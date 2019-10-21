using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Combat.Skill.Model;
using EventHorizon.Zone.System.Combat.Skill.State;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Combat.Skill.Load
{
    public struct LoadCombatSkillsEventHandler : INotificationHandler<LoadCombatSkillsEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;
        readonly ISkillRepository _skillRepository;

        public LoadCombatSkillsEventHandler(
            ILogger<LoadCombatSkillsEventHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            ISkillRepository skillRepository
        )
        {
            _logger = logger;
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
                try
                {
                    // TODO: Update this to recursive loop through Directories to load in skills.
                    var loadedSkill = await _fileLoader.GetFile<SkillInstance>(
                        skillFile.FullName
                    );
                    loadedSkill.Id = SkillInstance.CreateIdFromFileName(
                        skillFile.Name
                    );

                    _skillRepository.Set(
                        loadedSkill
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to load {FileName}.", skillFile.FullName);
                }
            }
        }
    }
}