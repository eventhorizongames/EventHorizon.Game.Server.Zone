namespace EventHorizon.Game.Server.Zone.I18n.Loader
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.I18n;
    using EventHorizon.Game.I18n.Loader;
    using EventHorizon.Game.I18n.Model;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using MediatR;

    public class I18nLoadHandler
        : INotificationHandler<I18nLoadEvent>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;
        readonly I18nRepository _i18nRepository;

        public I18nLoadHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            I18nRepository i18nRepository
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
            _i18nRepository = i18nRepository;
        }

        public async Task Handle(
            I18nLoadEvent notification,
            CancellationToken cancellationToken
        )
        {
            await LoadDirectoryIntoRepository(
                _serverInfo.I18nPath,
                cancellationToken
            );
        }

        public async Task LoadDirectoryIntoRepository(
            string path,
            CancellationToken cancellationToken
        )
        {
            if (!await _mediator.Send(
                new DoesDirectoryExist(
                    path
                ),
                cancellationToken
            ))
            {
                return;
            }
            // Read all i18n files of path
            foreach (var i18nFileInfo in await _mediator.Send(
                new GetListOfFilesFromDirectory(
                    path
                ),
                cancellationToken
            ))
            {
                // Add i18n translation list into i18nRepository
                var i18nFile = await _fileLoader.GetFile<I18nFile>(
                    i18nFileInfo.FullName
                );
                _i18nRepository.SetRepository(
                    i18nFile.Locale,
                    i18nFile.TranslationList
                );
            }
            // Read through the directories of path
            foreach (var directoryInfo in await _mediator.Send(
                new GetListOfDirectoriesFromDirectory(
                    path
                ),
                cancellationToken
            ))
            {
                await LoadDirectoryIntoRepository(
                    directoryInfo.FullName,
                    cancellationToken
                );
            }
        }
    }
}