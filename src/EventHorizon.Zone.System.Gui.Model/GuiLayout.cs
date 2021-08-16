using System.Collections.Generic;

namespace EventHorizon.Zone.System.Gui.Model
{
    public struct GuiLayout
    {
        public string Id { get; set; }
        public int Sort { get; set; }
        public int Layer { get; set; }
        public IList<GuiLayoutControl> ControlList { get; set; }
        public string InitializeScript { get; set; }
        public string ActivateScript { get; set; }
        public string DisposeScript { get; set; }
        public string UpdateScript { get; set; }
        public string DrawScript { get; set; }
    }
}
