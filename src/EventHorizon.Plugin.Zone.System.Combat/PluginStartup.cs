using EventHorizon.Game.Server.Zone.Plugin;
using EventHorizon.Plugin.Zone.System.Combat.Events;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat
{
    public class PluginStartup : IPluginStartup
    {
        public string[] DependentPluginList()
        {
            return new string[0];
        }

        public void Startup(IMediator mediator)
        {
            mediator.Publish(new SetupCombatSystemGuiEvent());
        }

        public string ValidateStartup()
        {
            return "good";
        }
    }
}