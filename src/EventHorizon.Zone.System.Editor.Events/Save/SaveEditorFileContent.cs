using System.Collections.Generic;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;

namespace EventHorizon.Zone.System.Editor.Events.Save
{
    public struct SaveEditorFileContent : IRequest<EditorResponse>
    {
        public IList<string> FilePath { get; }
        public string FileName { get; }
        public string Content { get; }

        public SaveEditorFileContent(
            IList<string> path,
            string fileName,
            string content
        )
        {
            FilePath = path;
            FileName = fileName;
            Content = content;
        }
    }
}