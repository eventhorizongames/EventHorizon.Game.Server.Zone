using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Gui;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Model.Gui;
using EventHorizon.Game.Server.Zone.Model.Gui.Options;
using EventHorizon.Plugin.Zone.System.Combat.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Handlers
{
    public class FillGuiDataListForPlayerWithLifeDataHandler : INotificationHandler<FillGuiDataListForPlayerEvent>
    {
        public Task Handle(FillGuiDataListForPlayerEvent notification, CancellationToken cancellationToken)
        {
            var playerLifeState = notification.Player.GetProperty<LifeState>(LifeState.PROPERTY_NAME);
            notification.DataListRef.Add(
                new GuiControlData
                {
                    ControlId = "HealthBar",
                    Options = new GuiBar
                    {
                        Percent = this.GetPercent(playerLifeState.Health, playerLifeState.MaxHealth),
                        Text = $"{playerLifeState.Health} / {playerLifeState.MaxHealth}"
                    }
                }
            );
            notification.DataListRef.Add(
                new GuiControlData
                {
                    ControlId = "MagicBar",
                    Options = new GuiBar
                    {
                        Percent = this.GetPercent(playerLifeState.Magic, playerLifeState.MaxMagic),
                        Text = $"{playerLifeState.Magic} / {playerLifeState.MaxMagic}"
                    }
                }
            );
            return Task.CompletedTask;
        }

        private int GetPercent(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                return 0;
            }
            return (numerator * 100) / denominator;
        }
    }
}