using EventHorizon.Game.I18n;
using EventHorizon.Game.Server.Zone.External.DateTimeService;
using EventHorizon.Game.Server.Zone.External.RandomNumber;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Script
{
    public interface IScriptServices
    {
        IMediator Mediator { get; }
        IRandomNumberGenerator Random { get; }
        IDateTimeService DateTime { get; }
        I18nLookup I18n { get; }
    }
}