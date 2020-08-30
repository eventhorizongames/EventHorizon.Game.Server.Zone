namespace EventHorizon.Game.Server.Zone
{
    using System;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Builders;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Logging;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Model;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Weikio.PluginFramework.Catalogs;

    public static class SystemClientScriptsPluginCompilerExtensions
    {
        public static IServiceCollection AddSystemClientScriptsPluginCompiler(
            this IServiceCollection services,
            Action<ClientScriptsPluginCompilerOptions> options
        )
        {
            var compilerOptions = new ClientScriptsPluginCompilerOptions
            {
                // Default/Fall-back settings
                ClientScriptsSdkPackage = "EventHorizon.Game.Client.Scripts.SDK-Dev",
                // Default/Fall-back settings
                ClientScriptsSdkPackageVersion = "0.0.0-dev*",
            };
            options(compilerOptions);

            return services
                .AddTransient<AssemblyBuilder, CSharpAssemblyBuilder>()
                .AddSystemClientScriptsPluginCompilerLoadSDK(
                    compilerOptions
                )
            ;
        }

        public static IApplicationBuilder UseSystemClientScriptsPluginCompiler(
            this IApplicationBuilder app
        )
        {
            return app;
        }

        internal static IServiceCollection AddSystemClientScriptsPluginCompilerLoadSDK(
            this IServiceCollection services,
            ClientScriptsPluginCompilerOptions options
        )
        {
            NugetPluginCatalogOptions.Defaults.LoggerFactory = () => new CompilerPackageLoaderNuGetLogger(services);

            return services.AddPluginFramework()
                .AddPluginCatalog(
                    new NugetPackagePluginCatalog(
                        options.ClientScriptsSdkPackage,
                        packageVersion: options.ClientScriptsSdkPackageVersion,
                        includePrerelease: true
                    )
                )
            ;
        }
    }
}