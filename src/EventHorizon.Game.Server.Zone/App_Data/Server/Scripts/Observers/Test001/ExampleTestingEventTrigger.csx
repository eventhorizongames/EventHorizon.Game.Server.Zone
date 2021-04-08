using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Observer.Model;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

// ExampleTest001_TestingEventTrigger
public class __SCRIPT__
    : ServerScript
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "testing-tag" };

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Server Script");

        services.ObserverState.Trigger<Test001_TestingEventObserver, Test001_TestingEvent>(
            new Test001_TestingEvent
            {
                EventMessage = $"[Triggered] :: Observer Test001_TestingEvent "
            }
        );

        return new StandardServerScriptResponse(
            true,
            "testing_Test001_Testing_interation"
        );
    }
}
