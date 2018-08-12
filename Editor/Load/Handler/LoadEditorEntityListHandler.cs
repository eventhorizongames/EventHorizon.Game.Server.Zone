using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Editor.Model;
using EventHorizon.Game.Server.Zone.Load;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Zone.Editor.Load.Handler
{
    public class LoadEditorEntityListHandler : IRequestHandler<LoadEditorEntityListEvent, IList<EditorEntity>>
    {
        readonly IHostingEnvironment _hostingEnvironment;
        readonly IJsonFileLoader _fileLoader;
        public LoadEditorEntityListHandler(IHostingEnvironment hostingEnvironment, IJsonFileLoader fileLoader)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileLoader = fileLoader;
        }
        public async Task<IList<EditorEntity>> Handle(LoadEditorEntityListEvent request, CancellationToken cancellationToken)
        {
            // TODO: Move loading to a Persistence service
            return (await _fileLoader.GetFile<EditorEntityStateFile>(GetAssetsFileName())).EntityList;
        }
        private string GetAssetsFileName()
        {
            return $"{_hostingEnvironment.ContentRootPath}/App_Data/Entity.state.json";
        }
    }
}