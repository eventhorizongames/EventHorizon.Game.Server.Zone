using EventHorizon.Game.Server.Zone.Plugin;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.Flee
{
    public class PluginStartup : IPluginStartup
    {
        public string[] DependentPluginList()
        {
            return new string[0];
        }

        public void Startup(IMediator mediator)
        {

        }

        public string ValidateStartup()
        {
            return "good";
        }
    }
}