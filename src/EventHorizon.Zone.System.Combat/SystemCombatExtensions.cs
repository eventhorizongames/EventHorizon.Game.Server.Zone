namespace EventHorizon.Game.Server.Zone.Core;

using EventHorizon.TimerService;
using EventHorizon.Zone.System.Combat.Level;
using EventHorizon.Zone.System.Combat.Level.Upgrade;
using EventHorizon.Zone.System.Combat.Life;
using EventHorizon.Zone.System.Combat.Load;
using EventHorizon.Zone.System.Combat.Model.Level;
using EventHorizon.Zone.System.Combat.Model.Life;
using EventHorizon.Zone.System.Combat.State;
using EventHorizon.Zone.System.Combat.Timer;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemCombatExtensions
{
    public static IServiceCollection AddSystemCombat(
        this IServiceCollection services
    )
    {
        return services
            .AddSingleton<IEntityQueue<ChangeEntityLife>, EntityQueue<ChangeEntityLife>>()
            .AddSingleton<IEntityQueue<EntityLevelUp>, EntityQueue<EntityLevelUp>>()
            .AddSingleton<ILevelStateUpgrade, LevelStateUpgrade>()
            .AddSingleton<ILifeStateChange, LifeStateChange>()

            .AddTransient<ITimerTask, UpdateEntityLifeTimer>()
            .AddTransient<ITimerTask, EntityLevelUpTimer>()
        ;
    }
    public static void UseSystemCombat(
        this IApplicationBuilder app
    )
    {
        using var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        serviceScope.ServiceProvider
            .GetRequiredService<IMediator>()
            .Publish(
                new LoadCombatSystemEvent()
            ).GetAwaiter().GetResult();
    }
}
