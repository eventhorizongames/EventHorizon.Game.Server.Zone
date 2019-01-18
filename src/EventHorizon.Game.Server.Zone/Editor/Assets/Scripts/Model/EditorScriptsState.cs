using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Editor.Assets.Scripts.Model
{
    public struct EditorScriptsState
    {
        public IList<EditorScriptFile> Actions { get; set; }
        public IList<EditorScriptFile> Effects { get; set; }
        public IList<EditorScriptFile> Routines { get; set; }
        public IList<EditorScriptFile> Server { get; set; }
        public IList<EditorScriptFile> Validators { get; set; }
    }
}