namespace EventHorizon.Zone.System.Gui.Api;

using EventHorizon.Zone.System.Gui.Model;

using global::System.Collections.Generic;

public interface GuiState
{
    void AddLayout(
        string id,
        GuiLayout layout
    );
    IEnumerable<GuiLayout> All();
    void Clear();
}
