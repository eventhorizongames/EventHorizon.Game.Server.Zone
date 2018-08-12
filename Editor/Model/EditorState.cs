using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Editor.Model
{
    public class EditorState
    {
        public IList<EditorAsset> AssetList { get; set; }
        public IList<EditorEntity> EntityList { get; set; }
        public IList<EditorScript> ScriptList { get; set; }
    }
}