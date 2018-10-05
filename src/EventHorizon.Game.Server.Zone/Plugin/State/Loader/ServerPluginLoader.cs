using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using EventHorizon.Game.Server.Zone.Plugin;
using EventHorizon.Game.Server.Zone.Plugin.Model;
using EventHorizon.Game.Server.Zone.Plugin.State;
using Microsoft.Extensions.Logging;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Plugin.State.Loader
{
    public class ServerPluginLoader : PluginLoader
    {
        readonly bool _inDebug;
        public ServerPluginLoader(
            bool inDebug)
        {
            _inDebug = inDebug;
        }
        public List<PluginStartupContainer> LoadPluginListFromDirectory(
            string pluginDirectory)
        {
            var pluginList = new List<PluginStartupContainer>();

            var filePathList = Directory.GetFiles(
                pluginDirectory);

            foreach (var filePath in filePathList)
            {
                var fileName = IOPath.GetFileNameWithoutExtension(
                    filePath);
                var pluginAssembly = LoadAssembly(
                    pluginDirectory,
                    fileName);
                var types = pluginAssembly
                    .GetTypes()
                    .Where(
                        a => typeof(
                            IPluginStartup).IsAssignableFrom(
                                a));
                if (types.Count() == 0)
                {
                    if (_inDebug)
                    {
                        throw new PluginNotFound(
                            "Plugin does not have a Startup Implementation.",
                            fileName,
                            filePath);
                    }
                    continue;
                }
                if (types.Count() > 1)
                {
                    if (_inDebug)
                    {
                        throw new ToManyPlugins(
                            "Plugin has more than one Startup Implementation.",
                            fileName,
                            filePath);
                    }
                    continue;
                }
                var pluginStartup = (IPluginStartup)Activator.CreateInstance(
                    types.First()
                );

                pluginList.Add(
                    new PluginStartupContainer
                    {
                        PluginName = fileName,
                        AssemblyInstance = pluginAssembly,
                        PluginStartup = pluginStartup,
                    });
            }

            return pluginList;
        }

        private Assembly LoadAssembly(
            string pluginDirectory,
            string fileName)
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