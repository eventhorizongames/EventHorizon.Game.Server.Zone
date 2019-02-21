using System.Collections;
using System.Collections.Generic;

namespace EventHorizon.Plugin.Zone.System.Combat.Load.Model
{
    public struct CombatSystemFile
    {
        public ISystemFile SkillScripts { get; set; }
        public ISystemFile Skills { get; set; }
        public IList<ISystemFile> GuiList { get; set; }
        public IList<ISystemFile> I18nList { get; set; }
        public IList<ISystemFile> ParticleList { get; set; }
    }

    public struct ISystemFile
    {
        public string File { get; set; }
    }
}