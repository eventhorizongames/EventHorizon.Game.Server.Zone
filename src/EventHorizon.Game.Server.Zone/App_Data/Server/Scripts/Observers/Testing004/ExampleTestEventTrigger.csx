using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Observer.Model;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

// ExampleTesting004_TestEventTrigger
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

        services.ObserverState.Trigger<Testing004_TestEventObserver, Testing004_TestEvent>(
            new Testing004_TestEvent
            {
                EventMessage = $"[Triggered] :: Observer Testing004_TestEvent "
            }
        );

        return new StandardServerScriptResponse(
            true,
            "testing_Testing004_Test_interation"
        );
    }
}
