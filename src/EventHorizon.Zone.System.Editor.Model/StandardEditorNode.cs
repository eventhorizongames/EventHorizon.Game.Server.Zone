using System;
using System.Collections.Generic;
using System.Text;

namespace EventHorizon.Zone.System.Editor.Model
{
    public struct StandardEditorNode : IEditorNode
    {
        public string Id { get; }
        public string Name { get; }
        public bool IsFolder { get; }
        public IList<string> Path { get; }
        public IList<IEditorNode> Children { get; }
        public string Type { get; }
        public IDictionary<string, object> Properties { get; }

        public StandardEditorNode(
            string name,
            bool isFolder,
            IList<string> path,
            string type
        )
        {
            Id = StandardEditorFile.GenerateId(
                name,
                path
            );
            Name = name;
            IsFolder = isFolder;
            Path = path;
            Type = type;
            Children = new List<IEditorNode>();
            Properties = new Dictionary<string, object>();
        }
        public IEditorNode AddProperty(
            string key,
            object value
        )
        {
            Properties.Add(
                key,
                value
            );
            return this;
        }
    }
}