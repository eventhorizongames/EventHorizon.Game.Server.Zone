using System.Collections.Generic;

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
        /// <summary>
        /// Root folder constructor, pass in the name and it will also be used as the Folder
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StandardEditorNode(
            string name
        ) : this(name, true, new string[] { }, "FOLDER") { }
        public IEditorNode AddProperty(
            string key,
            object value
        )
        {
            Properties[key] = value;
            return this;
        }
        public IEditorNode AddChild(
            IEditorNode child
        ) {
            Children.Add(
                child
            );
            return this;
        }
    }
}