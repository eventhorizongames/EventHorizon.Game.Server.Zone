using System.Collections.Concurrent;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Gui;

namespace EventHorizon.Game.Server.Zone.Gui.State
{
    public class GuiStateContainer : GuiState
    {
        private static readonly ConcurrentDictionary<string, GuiControlLayout> LAYOUT_MAP = new ConcurrentDictionary<string, GuiControlLayout>();
        private static readonly ConcurrentDictionary<string, GuiControlTemplate> TEMPLATE_MAP = new ConcurrentDictionary<string, GuiControlTemplate>();

        public void AddLayout(string id, GuiControlLayout layout)
        {
            LAYOUT_MAP.AddOrUpdate(id, layout, (key, old) => layout);
        }
        public IEnumerable<GuiControlLayout> AllLayouts()
        {
            return LAYOUT_MAP.Values;
        }

        public IEnumerable<GuiControlTemplate> All()
        {
            return TEMPLATE_MAP.Values;
        }
        public void Add(string id, GuiControlTemplate template)
        {
            TEMPLATE_MAP.AddOrUpdate(id, template, (key, oldEntity) => template);
        }
        public GuiControlTemplate Get(string id)
        {
            var template = default(GuiControlTemplate);
            TEMPLATE_MAP.TryGetValue(id, out template);
            return template;
        }
        public bool Contains(string id)
        {
            return TEMPLATE_MAP.ContainsKey(id);
        }
        public void Remove(string id)
        {
            var template = default(GuiControlTemplate);
            TEMPLATE_MAP.TryRemove(id, out template);
        }
        public void Update(string id, GuiControlTemplate template)
        {
            if (this.Contains(id))
            {
                TEMPLATE_MAP.AddOrUpdate(id, template, (key, oldEntity) => template);
            }
        }
    }
}