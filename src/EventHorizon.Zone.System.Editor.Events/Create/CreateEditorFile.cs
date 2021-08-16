using System.Collections.Generic;

using EventHorizon.Zone.System.Editor.Model;

using MediatR;

namespace EventHorizon.Zone.System.Editor.Events.Create
{
    public struct CreateEditorFile : IRequest<EditorResponse>
    {
        public IList<string> FilePath { get; }
        public string FileName { get; }

        public CreateEditorFile(
            IList<string> path,
            string fileName
        )
        {
            FilePath = path;
            FileName = fileName;
        }
    }
}
