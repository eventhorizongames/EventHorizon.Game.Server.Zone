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
                        Percent = this.GetPercent(playerLifeState.HealthPoints, playerLifeState.MaxHealthPoints),
                        Text = $"{playerLifeState.HealthPoints} / {playerLifeState.MaxHealthPoints}"
                    }
                }
            );
            notification.DataListRef.Add(
                new GuiControlData
                {
                    ControlId = "MagicBar",
                    Options = new GuiBar
                    {
                        Percent = this.GetPercent(playerLifeState.ActionPoints, playerLifeState.MaxActionPoints),
                        Text = $"{playerLifeState.ActionPoints} / {playerLifeState.MaxActionPoints}"
                    }
                }
            );
            return Task.CompletedTask;
        }

        private long GetPercent(long numerator, long denominator)
        {
            if (denominator == 0)
            {
                return 0;
            }
            return (numerator * 100) / denominator;
        }
    }
}