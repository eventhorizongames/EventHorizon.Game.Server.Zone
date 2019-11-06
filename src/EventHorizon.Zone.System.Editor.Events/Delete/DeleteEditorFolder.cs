using System.Collections.Generic;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;

namespace EventHorizon.Zone.System.Editor.Events.Delete
{
    public struct DeleteEditorFolder : IRequest<EditorResponse>
    {
        public IList<string> FolderPath { get; }
        public string FolderName { get; }

        public DeleteEditorFolder(
            IList<string> path,
            string folderName
        )
        {
            FolderPath = path;
            FolderName = folderName;
        }
    }
}