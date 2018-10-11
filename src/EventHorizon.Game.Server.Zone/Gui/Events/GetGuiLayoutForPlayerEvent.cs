using EventHorizon.Game.Server.Zone.Model.Gui;
using EventHorizon.Game.Server.Zone.Model.Player;
using EventHorizon.Game.Server.Zone.Player.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Gui.Events
{
    public struct GetGuiLayoutForPlayerEvent : IRequest<GuiLayout>
    {
        public PlayerEntity Player { get; set; }
    }
}