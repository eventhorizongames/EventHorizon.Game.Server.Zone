using System.Collections.Generic;

using EventHorizon.Zone.System.Editor.Model;

using MediatR;

namespace EventHorizon.Zone.System.Editor.Events.State
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
    }
}
