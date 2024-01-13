namespace EventHorizon.Game.Server.Zone.Load.Settings.Events.Handler;

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

public class LoadZoneSettingsHandler
    : INotificationHandler<LoadZoneSettingsEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;
    private readonly ServerInfo _serverInfo;
    private readonly IJsonFileLoader _fileLoader;
    private readonly IJsonFileSaver _fileSaver;
    private readonly IZoneSettingsSetter _zoneSettingsBuilder;
    private readonly IZoneSettingsFactory _zoneSettingsFactory;

    public LoadZoneSettingsHandler(
        ILogger<LoadZoneSettingsHandler> logger,
        IMediator mediator,
        ServerInfo serverInfo,
        IJsonFileLoader fileLoader,
        IJsonFileSaver fileSaver,
        IZoneSettingsSetter zoneSettingsBuilder,
        IZoneSettingsFactory zoneSettingsFactory
    )
    {
        _mediator = mediator;
        _logger = logger;
        _serverInfo = serverInfo;
        _fileLoader = fileLoader;
        _fileSaver = fileSaver;
        _zoneSettingsBuilder = zoneSettingsBuilder;
        _zoneSettingsFactory = zoneSettingsFactory;
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
            ),
            cancellationToken
        ))
        {
            _logger.LogWarning(
                "Settings file not found. {SettingsFile}",
                settingsFile
            );
            await _fileSaver.SaveToFile(
                Path.GetDirectoryName(settingsFile).ValidateForNull(),
                Path.GetFileName(settingsFile).ValidateForNull(),
                _zoneSettingsFactory.Settings
            );
            return;
        }
        var existingSettings = await _fileLoader.GetFile<ZoneSettings>(
            settingsFile
        );
        if (existingSettings.IsNull())
        {

            _logger.LogWarning(
                "Settings file was not valid. {@SettingsFile}",
                settingsFile
            );
            return;
        }

        _zoneSettingsBuilder.Set(
            existingSettings
        );
    }

    private string GetSettingsFileName()
    {
        return Path.Combine(
            _serverInfo.AppDataPath,
            "ZoneSettings.json"
        );
    }
}
