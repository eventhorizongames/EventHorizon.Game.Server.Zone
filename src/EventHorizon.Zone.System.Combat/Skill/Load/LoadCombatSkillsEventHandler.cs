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


            // Start Loading Skills from Root Client Skill Directory
            await this.LoadFromDirectoryInfo(
                _serverInfo.ClientPath + Path.DirectorySeparatorChar,
                new DirectoryInfo(
                    skillPath
                ),
                this.DoSomethingAsync
            );
        }
        
        public async Task DoSomethingAsync(
            string rootPath,
            DirectoryInfo directoryInfo,
            FileInfo fileInfo
        )
        {
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

        private async Task LoadFromDirectoryInfo(
            string rootPath,
            DirectoryInfo directoryInfo,
            Func<string, DirectoryInfo, FileInfo, Task> onFileInfo
        )
        {
            // Load Scripts from Sub-Directories
            foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
            {
                // Load Files From Directories
                await this.LoadFromDirectoryInfo(
                    rootPath,
                    subDirectoryInfo,
                    onFileInfo
                );
            }
            // Load script files into Repository
            await this.LoadFileIntoRepository(
                rootPath,
                directoryInfo,
                onFileInfo
            );
        }


        private async Task LoadFileIntoRepository(
            string rootPath,
            DirectoryInfo directoryInfo,
            Func<string, DirectoryInfo, FileInfo, Task> onFileInfo
        )
        {
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                await onFileInfo(
                    rootPath,
                    directoryInfo,
                    fileInfo
                );
            }
        }
    }
}