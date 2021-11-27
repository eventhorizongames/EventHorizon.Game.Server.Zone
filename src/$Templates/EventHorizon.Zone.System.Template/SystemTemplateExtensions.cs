namespace EventHorizon.Game.Server.Zone;

using System;

using EventHorizon.Zone.System.Template.Load;
using EventHorizon.Zone.System.Template.Model;
using EventHorizon.Zone.System.Template.Timer;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemTemplateExtensions
{
    public static IServiceCollection AddSystemTemplate(
        this IServiceCollection services,
        Action<SystemTemplateOptions> options
    )
    {
        // Default settings
        var scriptsOptions = new SystemTemplateOptions
        {
        };
        options(scriptsOptions);


        return services
            .AddSingleton(
                new TemplateSystemSettingsModel(
                    scriptsOptions
                )
            )

            .AddSingleton<TemplateBackgroundTimer>()
        ;
    }

    public static IApplicationBuilder UseSystemTemplate(
        this IApplicationBuilder app
    )
    {
        app.SendMediatorCommand<LoadTemplateSystemCommand, LoadTemplateSystemResult>(
            new LoadTemplateSystemCommand ()
        );

        return app;
    }
}
