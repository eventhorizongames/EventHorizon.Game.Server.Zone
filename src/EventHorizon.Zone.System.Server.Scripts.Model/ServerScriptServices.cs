using EventHorizon.Game.I18n;
using MediatR;

namespace EventHorizon.Zone.System.Server.Scripts.Model
{
    public interface ServerScriptServices
    {
        IMediator Mediator { get; }
        I18nLookup I18n { get; }
    }
}