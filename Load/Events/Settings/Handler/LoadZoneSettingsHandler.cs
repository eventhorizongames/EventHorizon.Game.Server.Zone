using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Load.Model;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Zone.Load.Events.Settings.Handler
{
    public class LoadZoneSettingsHandler : INotificationHandler<LoadZoneSettingsEvent>
    {
        readonly IHostingEnvironment _hostingEnvironment;
        readonly IZoneSettingsSetter _zoneSettingsBuilder;
        public LoadZoneSettingsHandler(IHostingEnvironment hostingEnvironment, IZoneSettingsSetter zoneSettingsBuilder)
        {
            _hostingEnvironment = hostingEnvironment;
            _zoneSettingsBuilder = zoneSettingsBuilder;
        }
        public async Task Handle(LoadZoneSettingsEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Move loading to a Persistence service
            if (SettingsFileExists())
            {
                using (var settingsFile = File.OpenText(GetSettingsFileName()))
                {
                    _zoneSettingsBuilder.Set(JsonConvert.DeserializeObject<ZoneSettings>(await settingsFile.ReadToEndAsync()));
                }
            }
        }
        private bool SettingsFileExists()
        {
            return File.Exists(GetSettingsFileName());
        }
        private string GetSettingsFileName()
        {
            return $"{_hostingEnvironment.ContentRootPath}/App_Data/ZoneSettings.json";
        }
    }
}