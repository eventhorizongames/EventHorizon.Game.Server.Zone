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
            // Default/Fall-back settings
            var compilerOptions = new ClientScriptsPluginCompilerOptions
            {
                SdkPackage = "EventHorizon.Game.Client.Scripts.SDK-Dev",
                SdkPackageVersion = "0.0.*",
                IncludePrerelease = true,
                NuGetFeed = "https://api.nuget.org/v3/index.json",
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
                        options.SdkPackage,
                        packageVersion: options.SdkPackageVersion,
                        includePrerelease: options.IncludePrerelease,
                        packageFeed: new NuGetFeed(
                            "personal-nuget-feed",
                            options.NuGetFeed
                        )
                    )
                )
            ;
        }
    }
}