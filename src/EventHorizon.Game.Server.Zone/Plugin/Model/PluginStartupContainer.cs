using System.Reflection;

namespace EventHorizon.Game.Server.Zone.Plugin.Model
{
    public struct PluginStartupContainer
        {
            public Assembly AssemblyInstance { get; internal set; }
            public string PluginName { get; internal set; }
            public IPluginStartup PluginStartup { get; internal set; }
        }
}