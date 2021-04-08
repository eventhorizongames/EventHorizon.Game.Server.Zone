using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Observer.Model;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

// ExampleTesting004_TestEventObserverHandler.csx
public class __SCRIPT__
    : ServerScript,
    Testing004_TestEventObserver
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "testing-tag" };

    private ServerScriptServices _services;
    private ILogger _logger;

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        _services = services;
        _logger = services.Logger<__SCRIPT__>();
        _logger.LogDebug("__SCRIPT__ - Server Script");

        _logger.LogInformation(
            $"I should be called to setup the Observer."
        );

        return new StandardServerScriptResponse(
            true,
            "testing_Testing004_Test_observer"
        );
    }

    public Task Handle(
        Testing004_TestEvent args
    )
    {
        var message = args.EventMessage;

        _logger.LogInformation(
            $"Message in Observer Handler: {message}"
        );

        return Task.CompletedTask;
    }
}
