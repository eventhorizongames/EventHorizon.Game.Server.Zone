using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using EventHorizon.Game.Server.Zone.Plugin;
using EventHorizon.Game.Server.Zone.Plugin.Model;
using EventHorizon.Game.Server.Zone.Plugin.State;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Plugin.State.Loader
{
    public class ServerPluginLoader : PluginLoader
    {
        public List<PluginStartupContainer> LoadPluginListFromDirectory(string pluginDirectory)
        {
            var pluginList = new List<PluginStartupContainer>();

            var filePathList = Directory.GetFiles(pluginDirectory);

            foreach (var filePath in filePathList)
            {
                var fileName = IOPath.GetFileNameWithoutExtension(filePath);
                var pluginAssembly = LoadAssembly(pluginDirectory, fileName);
                var pluginStartup = (IPluginStartup)Activator.CreateInstance(
                    pluginAssembly.GetType($"{fileName}.PluginStartup")
                );

                pluginList.Add(new PluginStartupContainer
                {
                    PluginName = fileName,
                    AssemblyInstance = pluginAssembly,
                    PluginStartup = pluginStartup,
                });
            }

            return pluginList;
        }

        private Assembly LoadAssembly(string pluginDirectory, string fileName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(
                IOPath.Combine(
                    pluginDirectory,
                    $"{fileName}.dll"
                )
            );
        }
    }
}