using EventHorizon.Game.I18n;
using EventHorizon.Game.Server.Zone.Server.Api;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Server.Model
{
    public class ServerScriptServices : IServerScriptServices
    {
        public IMediator Mediator { get; }
        public I18nLookup I18n { get; }

        public ServerScriptServices(
            IMediator mediator,
            I18nLookup i18n
        )
        {
            Mediator = mediator;
            I18n = i18n;
        }
    }
}