using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Plugin.Model;

namespace EventHorizon.Game.Server.Zone.Plugin.State
{
    public interface PluginState
    {
        List<PluginStartupContainer> PluginList { get; }
    }
}