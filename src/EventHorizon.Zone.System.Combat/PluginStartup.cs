namespace EventHorizon.Zone.System.Combat;

using EventHorizon.Zone.System.Combat.Load;

using MediatR;

public class PluginStartup
{
    public string[] DependentPluginList()
    {
        return new string[0];
    }

    public void Startup(IMediator mediator)
    {
        mediator.Publish(new LoadCombatSystemEvent());
    }

    public string ValidateStartup()
    {
        return "good";
    }
}
