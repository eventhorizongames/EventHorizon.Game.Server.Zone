using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Settings.Load;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Settings;
using MediatR;
using Newtonsoft.Json;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Load.Settings.Events.Handler
{
    public class LoadZoneSettingsHandler : INotificationHandler<LoadZoneSettingsEvent>
    {
        readonly ServerInfo _serverInfo;
        readonly IZoneSettingsSetter _zoneSettingsBuilder;
        public LoadZoneSettingsHandler(
            ServerInfo serverInfo,
            IZoneSettingsSetter zoneSettingsBuilder
        )
        {
            _serverInfo = serverInfo;
            _zoneSettingsBuilder = zoneSettingsBuilder;
        }
        public async Task Handle(LoadZoneSettingsEvent notification, CancellationToken cancellationToken)
        {
            if (SettingsFileExists())
            {
                using (var settingsFile = File.OpenText(GetSettingsFileName()))
                {
                    _zoneSettingsBuilder.Set(
                        JsonConvert.DeserializeObject<ZoneSettings>(
                            await settingsFile.ReadToEndAsync()
                        )
                    );
                }
            }
        }
        private bool SettingsFileExists()
        {
            return File.Exists(
                GetSettingsFileName()
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