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
        // public IList<EditorGui> GuiList { get; set; }
        // public IList<EditorModules> ModuleList { get; set; }
        // public IList<EditorParticle> ParticleList { get; set; }
        // public IList<EditorServerModule> ServerModuleList { get; set; }
        // public IList<EditorSkills> SkillsList { get; set; }
        public EditorScriptsState EditorScripts { get; set; }
        // public IEditor18n I18n { get; set; }
        // public IList<EditorServerRoutines> ServerRoutineList { get; set; }
    }
}