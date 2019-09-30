using EventHorizon.Game.I18n;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.RandomNumber;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Ai.Script
{
    public interface IScriptServices
    {
        IMediator Mediator { get; }
        IRandomNumberGenerator Random { get; }
        IDateTimeService DateTime { get; }
        I18nLookup I18n { get; }
    }
}