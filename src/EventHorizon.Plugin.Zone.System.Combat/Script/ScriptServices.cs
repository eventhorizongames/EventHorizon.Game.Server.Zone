using EventHorizon.Game.I18n;
using EventHorizon.Game.Server.Zone.External.DateTimeService;
using EventHorizon.Game.Server.Zone.External.RandomNumber;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Script
{
    public class ScriptServices : IScriptServices
    {
        public IMediator Mediator { get; private set; }
        public IRandomNumberGenerator Random { get; private set; }
        public IDateTimeService DateTime { get; private set; }
        public I18nLookup I18n { get; private set; }

        public ScriptServices(
            IMediator mediator,
            IDateTimeService dateTime,
            IRandomNumberGenerator random,
            I18nLookup i18n
        )
        {
            Mediator = mediator;
            DateTime = dateTime;
            Random = random;
            I18n = i18n;
        }
    }
}