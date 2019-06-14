using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Json;
using EventHorizon.Game.Server.Zone.Editor.Model;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Game.Server.Zone.Load;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Zone.Editor.Load.Handler
{
    public class LoadEditorAssetListHandler : IRequestHandler<LoadEditorAssetListEvent, IList<EditorAsset>>
    {
        readonly IHostingEnvironment _hostingEnvironment;
        readonly IJsonFileLoader _fileLoader;
        public LoadEditorAssetListHandler(IHostingEnvironment hostingEnvironment, IJsonFileLoader fileLoader)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileLoader = fileLoader;
        }
        public async Task<IList<EditorAsset>> Handle(LoadEditorAssetListEvent request, CancellationToken cancellationToken)
        {
            // TODO: Need to change this to load from App_Data/Client/Assets
            return (await _fileLoader.GetFile<EditorAssetStateFile>(GetAssetsFileName())).AssetList;
        }
        private string GetAssetsFileName()
        {
            return $"{_hostingEnvironment.ContentRootPath}/App_Data/Asset.state.json";
        }
    }
}