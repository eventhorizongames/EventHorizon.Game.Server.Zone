using System.Collections.Generic;

using EventHorizon.Zone.System.Gui.Model;

namespace EventHorizon.Zone.System.Gui.Api
{
    public interface GuiState
    {
        void AddLayout(
            string id,
            GuiLayout layout
        );
        IEnumerable<GuiLayout> All();
    }
}
