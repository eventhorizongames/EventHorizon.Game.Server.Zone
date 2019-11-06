using System.Collections.Generic;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;

namespace EventHorizon.Zone.System.Editor.Events.Create
{
    public struct CreateEditorFolder : IRequest<EditorResponse>
    {
        public IList<string> FilePath { get; }
        public string FolderName { get; }

        public CreateEditorFolder(
            IList<string> path,
            string folderName
        )
        {
            FilePath = path;
            FolderName = folderName;
        }
    }
}