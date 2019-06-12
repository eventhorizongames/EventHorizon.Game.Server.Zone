using EventHorizon.Game.I18n;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Server.Api
{
    public interface IServerScriptServices
    {
        IMediator Mediator { get; }
        I18nLookup I18n { get; }
    }
}