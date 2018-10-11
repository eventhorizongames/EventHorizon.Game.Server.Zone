using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Json;
using EventHorizon.Game.Server.Zone.Editor.Model;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Game.Server.Zone.Load;
using EventHorizon.Game.Server.Zone.Load.Map.Model;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Zone.Editor.Load.Handler
{
    public class LoadEditorZoneMapHandler : IRequestHandler<LoadEditorZoneMapEvent, ZoneMap>
    {
        readonly IHostingEnvironment _hostingEnvironment;
        readonly IJsonFileLoader _fileLoader;
        public LoadEditorZoneMapHandler(IHostingEnvironment hostingEnvironment, IJsonFileLoader fileLoader)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileLoader = fileLoader;
        }
        public async Task<ZoneMap> Handle(LoadEditorZoneMapEvent request, CancellationToken cancellationToken)
        {
            // TODO: Move loading to a Persistence service
            return await _fileLoader.GetFile<ZoneMap>(GetAssetsFileName());
        }
        private string GetAssetsFileName()
        {
            return $"{_hostingEnvironment.ContentRootPath}/App_Data/Map.state.json";
        }
    }
}