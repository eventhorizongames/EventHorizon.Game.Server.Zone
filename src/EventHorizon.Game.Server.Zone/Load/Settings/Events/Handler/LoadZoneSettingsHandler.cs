using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Settings.Load;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.Core.Model.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Load.Settings.Events.Handler
{
    public class LoadZoneSettingsHandler : INotificationHandler<LoadZoneSettingsEvent>
    {
        readonly IMediator _mediator;
        readonly ILogger _logger;
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;
        readonly IZoneSettingsSetter _zoneSettingsBuilder;

        public LoadZoneSettingsHandler(
            ILogger<LoadZoneSettingsHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            IZoneSettingsSetter zoneSettingsBuilder
        )
        {
            _mediator = mediator;
            _logger = logger;
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
            _zoneSettingsBuilder = zoneSettingsBuilder;
        }

        public async Task Handle(
            LoadZoneSettingsEvent notification,
            CancellationToken cancellationToken
        )
        {
            var settingsFile = GetSettingsFileName();
            if (!await _mediator.Send(
                new DoesFileExist(
                    settingsFile
                )
            ))
            {
                _logger.LogWarning(
                    "Settings file not found. {SettingsFile}",
                    settingsFile
                );
                return;
            }
            _zoneSettingsBuilder.Set(
                await _fileLoader.GetFile<ZoneSettings>(
                    settingsFile
                )
            );
        }

        private string GetSettingsFileName()
        {
            return IOPath.Combine(
                _serverInfo.AppDataPath,
                "ZoneSettings.json"
            );
        }
    }
}