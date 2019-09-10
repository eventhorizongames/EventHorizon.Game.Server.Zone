using EventHorizon.Game.I18n;
using EventHorizon.Zone.System.Server.Scripts.Model;
using MediatR;

namespace EventHorizon.Zone.System.Server.Scripts.System
{
    public class SystemServerScriptServices : ServerScriptServices
    {
        public IMediator Mediator { get; }
        public I18nLookup I18n { get; }

        public SystemServerScriptServices(
            IMediator mediator,
            I18nLookup i18n
        )
        {
            Mediator = mediator;
            I18n = i18n;
        }
    }
}