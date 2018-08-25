using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Json;
using EventHorizon.Game.Server.Zone.Editor.Model;
using EventHorizon.Game.Server.Zone.Load;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Zone.Editor.Load.Handler
{
    public class LoadEditorScriptListHandler : IRequestHandler<LoadEditorScriptListEvent, IList<EditorScript>>
    {
        readonly IHostingEnvironment _hostingEnvironment;
        readonly IJsonFileLoader _fileLoader;
        public LoadEditorScriptListHandler(IHostingEnvironment hostingEnvironment, IJsonFileLoader fileLoader)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileLoader = fileLoader;
        }
        public async Task<IList<EditorScript>> Handle(LoadEditorScriptListEvent request, CancellationToken cancellationToken)
        {
            // TODO: Move loading to a Persistence service
            return (await _fileLoader.GetFile<EditorScriptStateFile>(GetAssetsFileName())).ScriptList;
        }
        private string GetAssetsFileName()
        {
            return $"{_hostingEnvironment.ContentRootPath}/App_Data/Script.state.json";
        }
    }
}