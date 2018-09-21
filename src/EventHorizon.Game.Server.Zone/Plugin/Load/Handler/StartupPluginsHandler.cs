using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Plugin.State;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Plugin.Load.Handler
{
    public class StartupPluginsHandler : INotificationHandler<StartupPluginsEvent>
    {
        readonly IMediator _mediator;
        readonly PluginState _pluginState;

        public StartupPluginsHandler(IMediator mediator, PluginState pluginState)
        {
            _mediator = mediator;
            _pluginState = pluginState;
        }

        public Task Handle(StartupPluginsEvent notification, CancellationToken cancellationToken)
        {
            _pluginState.PluginList.ForEach(plugin => plugin.PluginStartup.Startup(_mediator));
            
            return Task.CompletedTask;
        }
    }
}