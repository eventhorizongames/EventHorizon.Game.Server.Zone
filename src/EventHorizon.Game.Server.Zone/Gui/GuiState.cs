using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Gui;

namespace EventHorizon.Game.Server.Zone.Gui
{
    public interface GuiState
    {
        void AddLayout(string id, GuiControlLayout layout);
        IEnumerable<GuiControlLayout> AllLayouts();

        IEnumerable<GuiControlTemplate> All();
        void Add(string id, GuiControlTemplate template);
        GuiControlTemplate Get(string id);
        bool Contains(string id);
        void Remove(string id);
        void Update(string id, GuiControlTemplate template);
    }
}