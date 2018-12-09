using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.I18n.Model;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using MediatR;

namespace EventHorizon.Game.I18n.Loader
{
    public class I18nLoadHandler : INotificationHandler<I18nLoadEvent>
    {
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;
        readonly I18nRepository _i18nRepository;

        public I18nLoadHandler(
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            I18nRepository i18nRepository
        )
        {
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
            _i18nRepository = i18nRepository;
        }

        public async Task Handle(
            I18nLoadEvent notification,
            CancellationToken cancellationToken
        )
        {
            var loadingDirectory = Path.Combine(_serverInfo.AssetsPath, "I18n");
            // Read all i18n files from Asset/I18n
            foreach (var i18nFileName in Directory.EnumerateFiles(loadingDirectory))
            {
                // Add i18n translation list into i18nRepository
                var i18nFile = await _fileLoader.GetFile<I18nFile>(
                    i18nFileName
                );
                _i18nRepository.SetRepository(
                    i18nFile.Locale,
                    i18nFile.TranslationList
                );
            }
        }
    }
}