using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Editor.Assets.Scripts.Model
{
    public class EditorScriptFileContent
    {
        public IList<string> FileDirectory { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
    }
}