using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using EventHorizon.Game.Server.Zone.Plugin.Model;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Plugin.State.Server
{
    public class ServerPluginState : PluginState
    {
        public static readonly int MAX_STEPS = 20;

        private List<PluginStartupContainer> _pluginList = new List<PluginStartupContainer>();
        public List<PluginStartupContainer> PluginList
        {
            get
            {
                return _pluginList.ToList();
            }
        }

        readonly IHostingEnvironment _hostingEnvironment;
        readonly PluginLoader _pluginLoader;
        readonly IServiceCollection _services;

        public ServerPluginState(IHostingEnvironment hostingEnvironment, IServiceCollection services, PluginLoader pluginLoader)
        {
            _hostingEnvironment = hostingEnvironment;
            _pluginLoader = pluginLoader;
            this.LoadPluginList(this.GeneratePluginsDirectory());
        }

        private string GeneratePluginsDirectory()
        {
            return IOPath.Combine(_hostingEnvironment.ContentRootPath, "App_Data", "Plugins");
        }

        private void LoadPluginList(string pluginsDirectory)
        {
            // Load all PluginList from Plugins Folder and Organize Plugin List by Dependencies of Assembly 
            _pluginList = SortPluginList(_pluginLoader.LoadPluginListFromDirectory(pluginsDirectory));

            // Initialize Services of Plugin List
            _pluginList.ForEach(plugin => _services.AddMediatR(plugin.AssemblyInstance));
        }

        List<PluginStartupContainer> SortPluginList(List<PluginStartupContainer> pluginList)
        {
            var responseList = new List<PluginStartupContainer>();

            pluginList.ForEach(plugin => AddPluginAndDependencies(responseList, pluginList, plugin, 0));

            return responseList;
        }

        private List<PluginStartupContainer> AddPluginAndDependencies(List<PluginStartupContainer> responseList, List<PluginStartupContainer> pluginList, PluginStartupContainer plugin, int step)
        {
            if (step > MAX_STEPS)
            {
                throw new Exception("To many steps loading assembly");
            }
            var dependentPluginList = plugin.PluginStartup.DependentPluginList();
            foreach (var pluginName in dependentPluginList)
            {
                if (ContainsPlugin(responseList, pluginName))
                {
                    continue;
                }
                else
                {
                    AddPluginAndDependencies(responseList, pluginList, FindPlugin(pluginList, pluginName), step++);
                }
            }
            responseList = AddPlugin(responseList, plugin);
            return responseList;
        }

        private PluginStartupContainer FindPlugin(List<PluginStartupContainer> pluginList, string pluginName)
        {
            return pluginList.First(a => a.PluginName == pluginName);
        }

        private static List<PluginStartupContainer> AddPlugin(List<PluginStartupContainer> responseList, PluginStartupContainer plugin)
        {
            if (!ContainsPlugin(responseList, plugin.PluginName))
            {
                responseList.Add(plugin);
            }
            return responseList;
        }

        private static bool ContainsPlugin(List<PluginStartupContainer> pluginList, string pluginName)
        {
            return pluginList.Any(a => a.PluginName == pluginName);
        }
    }
}