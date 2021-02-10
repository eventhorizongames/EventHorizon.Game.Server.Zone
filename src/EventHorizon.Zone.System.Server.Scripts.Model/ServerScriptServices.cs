namespace EventHorizon.Zone.System.Server.Scripts.Model
{
    using EventHorizon.Game.I18n;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.RandomNumber;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public interface ServerScriptServices
    {
        ServerInfo ServerInfo { get; }
        IMediator Mediator { get; }
        IRandomNumberGenerator Random { get; }
        IDateTimeService DateTime { get; }
        I18nLookup I18n { get; }
        ILogger Logger<T>();
    }
}
