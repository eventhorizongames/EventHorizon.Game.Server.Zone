using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Observer.Model;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

// ExampleTesting05_TEstingEventTrigger
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

        services.ObserverState.Trigger<Testing05_TEstingEventObserver, Testing05_TEstingEvent>(
            new Testing05_TEstingEvent
            {
                EventMessage = $"[Triggered] :: Observer Testing05_TEstingEvent "
            }
        );

        return new StandardServerScriptResponse(
            true,
            "testing_Testing05_TEsting_interation"
        );
    }
}
