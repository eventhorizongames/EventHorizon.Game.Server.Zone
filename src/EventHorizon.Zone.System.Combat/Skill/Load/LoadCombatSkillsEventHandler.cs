using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Combat.Skill.Model;
using EventHorizon.Zone.System.Combat.Skill.State;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Combat.Skill.Load
{
    public class LoadCombatSkillsEventHandler : INotificationHandler<LoadCombatSkillsEvent>
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

        public Task Handle(
            LoadCombatSkillsEvent notification,
            CancellationToken cancellationToken
        ) => _mediator.Send(
            new LoadFileRecursivelyFromDirectory(
                Path.Combine(
                    _serverInfo.ClientPath,
                    "Skills"
                ),
                OnProcessFile,
                new Dictionary<string, object>
                {
                    {
                        "RootPath",
                        // $"{_serverInfo.ClientPath}{Path.DirectorySeparatorChar}"
                        _serverInfo.ClientPath
                    }
                }
            )
        );

        private async Task OnProcessFile(
            StandardFileInfo fileInfo,
            IDictionary<string, object> arguments
        )
        {
            var rootPath = arguments["RootPath"] as string;
            var loadedSkill = await _fileLoader.GetFile<SkillInstance>(
                fileInfo.FullName
            );

            loadedSkill.Id = SkillInstance.GenerateId(
                rootPath.MakePathRelative(
                    fileInfo.DirectoryName
                ),
                fileInfo.Name
            );

            _skillRepository.Set(
                loadedSkill
            );
        }
    }
}