using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;

namespace EventHorizon.Zone.System.Editor.State
{
    public struct GetEditorFileContent : IRequest<StandardEditorFile>
    {
        public IList<string> FilePath { get; }
        public string FileName { get; }
        public GetEditorFileContent(
            IList<string> filePath,
            string fileName
        )
        {
            FilePath = filePath;
            FileName = fileName;
        }

        /// <summary>
        /// This Handler will get the file content based on the path and filename from the App_Data directory 
        /// </summary>
        public struct GetEditorFileContentHandler : IRequestHandler<GetEditorFileContent, StandardEditorFile>
        {
            readonly ServerInfo _serverInfo;
            public GetEditorFileContentHandler(
                ServerInfo serverInfo
            )
            {
                _serverInfo = serverInfo;
            }
            public Task<StandardEditorFile> Handle(
                GetEditorFileContent request,
                CancellationToken cancellationToken
            )
            {
                var filePath = Path.Combine(
                    _serverInfo.AppDataPath,
                    Path.Combine(
                        request.FilePath.ToArray()
                    ),
                    request.FileName
                );
                if (File.Exists(
                    filePath
                ))
                {
                    return Task.FromResult(
                        new StandardEditorFile(
                            request.FileName,
                            request.FilePath,
                            File.ReadAllText(
                                filePath
                            )
                        )
                    );
                }
                return Task.FromResult(
                    new StandardEditorFile(
                        "__invalid__",
                        new string[] { "__invalid__" },
                        "__invalid__"
                    )
                );
            }
        }
    }
}