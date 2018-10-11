
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Game.Server.Zone.Model.Gui;

namespace EventHorizon.Game.Server.Zone.Gui.Client
{
    public struct GuiAddControlData : IClientActionData
    {
        public string Id { get; set; }
        public GuiControlTemplate Template { get; set; }
    }
}