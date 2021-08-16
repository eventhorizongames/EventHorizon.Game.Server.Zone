
using System.Collections.Generic;
using System.Text;

namespace EventHorizon.Zone.System.Editor.Model
{
    public class StandardEditorFile
    {
        public string Id { get; }
        public string Name { get; }
        public IList<string> Path { get; }
        public string Content { get; }

        public StandardEditorFile(
            string fileName,
            IList<string> filePath,
            string fileContent
        )
        {
            Id = GenerateId(
                fileName,
                filePath
            );
            Name = fileName;
            Path = filePath;
            Content = fileContent;
        }

        public static string GenerateId(
            string name,
            IList<string> path
        )
        {
            var idStringBuilder = new StringBuilder();
            foreach (var pathPart in path)
            {
                idStringBuilder.Append(
                    pathPart
                ).Append(
                    "_"
                );
            }
            return idStringBuilder.Append(
                name
            ).ToString();
        }
    }
}
