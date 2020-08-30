namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Load;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Consolidate;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.CSharp;
    using EventHorizon.Zone.System.Client.Scripts.State;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemClientScriptsExtensions
    {
        public static IServiceCollection AddSystemClientScripts(
            this IServiceCollection services
        ) => services
            .AddSingleton<ClientScriptsState, InMemoryClientScriptsState>()
            .AddSingleton<ClientScriptRepository, ClientScriptInMemoryRepository>()

            .AddSingleton<ClientScriptsConsolidator, StandardClientScriptsConsolidator>()
            .AddSingleton<ClientScriptCompiler, ClientScriptCompilerForCSharp>()
        ;

        public static IApplicationBuilder UseSystemClientScripts(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Send(new LoadClientScriptsSystemCommand())
                    .GetAwaiter()
                    .GetResult();
            }
            return app;
        }
    }
}