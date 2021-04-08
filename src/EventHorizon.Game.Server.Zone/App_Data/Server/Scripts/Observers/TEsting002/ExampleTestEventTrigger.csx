using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Observer.Model;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

// ExampleTEsting002_TestEventTrigger
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

        services.ObserverState.Trigger<TEsting002_TestEventObserver, TEsting002_TestEvent>(
            new TEsting002_TestEvent
            {
                EventMessage = $"[Triggered] :: Observer TEsting002_TestEvent "
            }
        );

        return new StandardServerScriptResponse(
            true,
            "testing_TEsting002_Test_interation"
        );
    }
}
