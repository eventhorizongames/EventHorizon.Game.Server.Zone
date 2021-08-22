namespace EventHorizon.Zone.System.Gui.State
{
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;

    using EventHorizon.Zone.System.Gui.Api;
    using EventHorizon.Zone.System.Gui.Model;

    public class InMemoryGuiState : GuiState
    {
        private static readonly ConcurrentDictionary<string, GuiLayout> LAYOUT_MAP = new ConcurrentDictionary<string, GuiLayout>();

        public void AddLayout(
            string id,
            GuiLayout layout
        )
        {
            LAYOUT_MAP.AddOrUpdate(
                id,
                layout,
                (key, old) => layout
            );
        }

        public IEnumerable<GuiLayout> All()
        {
            return LAYOUT_MAP.Values;
        }
    }
}
