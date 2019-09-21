using EventHorizon.Game.Server.Zone.Plugin;
using EventHorizon.Zone.System.Combat.Events;
using EventHorizon.Zone.System.Combat.Load;
using MediatR;

namespace EventHorizon.Zone.System.Combat
{
    public class PluginStartup : IPluginStartup
    {
        public string[] DependentPluginList()
        {
            return new string[0];
        }

        public void Startup(IMediator mediator)
        {
            mediator.Publish(new LoadCombatSystemEvent());
        }

        public string ValidateStartup()
        {
            return "good";
        }
    }
}