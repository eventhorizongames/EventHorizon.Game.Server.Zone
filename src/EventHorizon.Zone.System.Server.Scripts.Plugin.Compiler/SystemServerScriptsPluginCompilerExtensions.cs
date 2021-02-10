namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Builders;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.CSharp;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemServerScriptsPluginCompilerExtensions
    {
        public static IServiceCollection AddSystemServerScriptsPluginCompiler(
            this IServiceCollection services
        )
        {
            return services
                .AddTransient<AssemblyBuilder, CSharpAssemblyBuilder>()
                .AddSingleton<ServerScriptCompiler, ServerScriptCompilerForCSharp>()
            ;
        }

        public static IApplicationBuilder UseSystemClientScriptsPluginCompiler(
            this IApplicationBuilder app
        )
        {
            return app;
        }
    }
}