namespace EventHorizon.Zone.System.Gui.Api
{
    using global::System.Collections.Generic;

    using EventHorizon.Zone.System.Gui.Model;

    public interface GuiState
    {
        void AddLayout(
            string id,
            GuiLayout layout
        );
        IEnumerable<GuiLayout> All();
    }
}
