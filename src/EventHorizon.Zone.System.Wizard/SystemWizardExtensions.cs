﻿namespace EventHorizon.Game.Server.Zone;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Wizard.Api;
using EventHorizon.Zone.System.Wizard.Load;
using EventHorizon.Zone.System.Wizard.State;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemWizardExtensions
{
    public static IServiceCollection AddSystemWizard(
        this IServiceCollection services
    ) => services
        .AddSingleton<WizardRepository, StandardWizardRepository>()
    ;

    public static IApplicationBuilder UseSystemWizard(
        this IApplicationBuilder app
    )
    {
        return app.SendMediatorCommand<LoadSystemsWizardListCommand, StandardCommandResult>(
            new LoadSystemsWizardListCommand()
        ).SendMediatorCommand<LoadWizardListCommand, StandardCommandResult>(
            new LoadWizardListCommand()
        );
    }
}
