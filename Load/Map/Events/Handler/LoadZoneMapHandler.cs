
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Load.Map.Model;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Zone.Load.Map.Events.Handler
{
    public class LoadZoneMapHandler : INotificationHandler<LoadZoneMapEvent>
    {
        readonly IHostingEnvironment _hostingEnvironment;
        readonly IZoneMapSetter _zoneMapBuilder;
        public LoadZoneMapHandler(IHostingEnvironment hostingEnvironment, IZoneMapSetter zoneMapBuilder)
        {
            _hostingEnvironment = hostingEnvironment;
            _zoneMapBuilder = zoneMapBuilder;
        }
        public async Task Handle(LoadZoneMapEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Move loading to a Persistence service
            if (MapFileExists())
            {
                using (var mapFile = File.OpenText(GetMapFileName()))
                {
                    _zoneMapBuilder.Set(JsonConvert.DeserializeObject<ZoneMap>(await mapFile.ReadToEndAsync()));
                }
            }
        }
        private bool MapFileExists()
        {
            return File.Exists(GetMapFileName());
        }
        private string GetMapFileName()
        {
            return $"{_hostingEnvironment.ContentRootPath}/App_Data/Map.state.json";
        }
    }
}