using EventHorizon.Game.I18n;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.RandomNumber;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Script
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