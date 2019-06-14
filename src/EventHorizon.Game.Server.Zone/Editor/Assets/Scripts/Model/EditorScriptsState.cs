using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Editor.Assets.Scripts.Model
{
    public struct EditorScriptsState
    {
        public IList<EditorScriptFile> Server { get; set; }
        public IList<EditorScriptFile> Client { get; set; }
    }
}