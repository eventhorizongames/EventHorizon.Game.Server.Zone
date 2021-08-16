using System.Collections.Generic;

using EventHorizon.Zone.System.Editor.Model;

using MediatR;

namespace EventHorizon.Zone.System.Editor.Events.Delete
{
    public class DeleteEditorFile : IRequest<EditorResponse>
    {
        public IList<string> FilePath { get; }
        public string FileName { get; }

        public DeleteEditorFile(
            IList<string> path,
            string fileName
        )
        {
            FilePath = path;
            FileName = fileName;
        }
    }
}
