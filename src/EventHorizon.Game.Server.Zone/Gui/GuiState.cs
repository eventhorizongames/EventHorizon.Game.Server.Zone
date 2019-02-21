using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Gui;

namespace EventHorizon.Game.Server.Zone.Gui
{
    public interface GuiState
    {
        void AddLayout(string id, GuiControlLayout layout);
        IEnumerable<GuiControlLayout> AllLayouts();

        IEnumerable<GuiTemplate> All();
        void Add(string id, GuiTemplate template);
        GuiTemplate Get(string id);
        bool Contains(string id);
        void Remove(string id);
        void Update(string id, GuiTemplate template);
    }
}