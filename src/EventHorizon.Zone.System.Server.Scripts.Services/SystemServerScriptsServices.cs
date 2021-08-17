using EventHorizon.Game.I18n;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.RandomNumber;

using MediatR;

public class Services
{
    public static ServerInfo ServerInfo = null!;
    public static IMediator Mediator = null!;
    public static I18nLookup I18n = null!;
    public static IRandomNumberGenerator Random = null!;
    public static IDateTimeService DateTime = null!;
}


public static class Data
{
    public static T? Get<T>(
#pragma warning disable IDE0060 // Remove unused parameter
        string name
#pragma warning restore IDE0060 // Remove unused parameter
    ) => default;
}
