namespace EventHorizon.Zone.System.Gui.State;

using EventHorizon.Zone.System.Gui.Api;
using EventHorizon.Zone.System.Gui.Model;

using global::System.Collections.Concurrent;
using global::System.Collections.Generic;

public class InMemoryGuiState
    : GuiState
{
    private readonly ConcurrentDictionary<string, GuiLayout> _layoutMap = new();

    public void AddLayout(
        string id,
        GuiLayout layout
    )
    {
        _layoutMap.AddOrUpdate(
            id,
            layout,
            (_, _) => layout
        );
    }

    public IEnumerable<GuiLayout> All()
    {
        return _layoutMap.Values;
    }

    public void Clear()
    {
        _layoutMap.Clear();
    }
}
