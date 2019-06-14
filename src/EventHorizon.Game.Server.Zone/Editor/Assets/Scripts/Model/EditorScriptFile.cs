using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Editor.Assets.Scripts.Model
{
    public struct EditorScriptFile
    {
        public IList<string> FileDirectory { get; set; }
        public string FileName { get; set; }
    }
}