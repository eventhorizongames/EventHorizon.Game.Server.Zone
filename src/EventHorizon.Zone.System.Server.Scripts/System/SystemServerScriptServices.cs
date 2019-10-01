using EventHorizon.Game.I18n;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.RandomNumber;
using EventHorizon.Zone.System.Server.Scripts.Model;
using MediatR;

namespace EventHorizon.Zone.System.Server.Scripts.System
{
    public class SystemServerScriptServices : ServerScriptServices
    {
        public IMediator Mediator { get; }
        public IRandomNumberGenerator Random { get; }
        public IDateTimeService DateTime { get; }
        public I18nLookup I18n { get; }

        public SystemServerScriptServices(
            IMediator mediator,
            IRandomNumberGenerator random,
            IDateTimeService dateTime,
            I18nLookup i18n
        )
        {
            Mediator = mediator;
            Random = random;
            DateTime = dateTime;
            I18n = i18n;
        }
    }
}