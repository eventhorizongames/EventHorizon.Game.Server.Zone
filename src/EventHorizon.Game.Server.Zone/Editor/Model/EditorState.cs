using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Editor.Assets.Scripts.Model;
using EventHorizon.Game.Server.Zone.Load.Map.Model;

namespace EventHorizon.Game.Server.Zone.Editor.Model
{
    public class EditorState
    {
        public ZoneMap Map { get; set; }
        public IList<EditorAsset> AssetList { get; set; }
        public IList<EditorEntity> EntityList { get; set; }
        public IList<EditorScript> ScriptList { get; set; }
        public EditorScriptsState EditorScripts { get; set; }
    }
}