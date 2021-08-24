namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Load
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.State;

    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class LoadCombatSkillsEventHandler
        : INotificationHandler<LoadCombatSkillsEvent>
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly IJsonFileLoader _fileLoader;
        private readonly SkillRepository _skillRepository;

        public LoadCombatSkillsEventHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            SkillRepository skillRepository
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
            _skillRepository = skillRepository;
        }

        public Task Handle(
            LoadCombatSkillsEvent notification,
            CancellationToken cancellationToken
        ) => _mediator.Send(
            new ProcessFilesRecursivelyFromDirectory(
                Path.Combine(
                    _serverInfo.ClientPath,
                    "Skills"
                ),
                OnProcessFile,
                new Dictionary<string, object>
                {
                    {
                        "RootPath",
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
                rootPath!.MakePathRelative(
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
