namespace EventHorizon.Zone.System.Editor.Events.Create
{
    using global::System.Collections.Generic;

    using EventHorizon.Zone.System.Editor.Model;

    using MediatR;

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
