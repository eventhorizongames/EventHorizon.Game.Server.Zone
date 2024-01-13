namespace EventHorizon.Game.Server.Zone;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemCombatPluginSkillEditorExtensions
{
    public static IServiceCollection AddSystemCombatPluginSkillEditor(
        this IServiceCollection services
    ) => services;

    public static IApplicationBuilder UseSystemCombatPluginSkillEditor(
        this IApplicationBuilder app
    ) => app;
}
