namespace EventHorizon.Zone.System.Gui.Model;

using global::System.Collections.Generic;

public class GuiLayout
{
    public string Id { get; set; } = string.Empty;
    public int Sort { get; set; }
    public int Layer { get; set; }
    public IList<GuiLayoutControl> ControlList { get; set; } = new List<GuiLayoutControl>();
    public string? InitializeScript { get; set; }
    public string? ActivateScript { get; set; }
    public string? DisposeScript { get; set; }
    public string? UpdateScript { get; set; }
    public string? DrawScript { get; set; }
}
