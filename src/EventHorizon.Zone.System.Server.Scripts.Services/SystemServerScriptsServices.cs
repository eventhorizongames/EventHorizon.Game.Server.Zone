using EventHorizon.Game.I18n;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.RandomNumber;
using MediatR;

public class Services
{
    public static ServerInfo ServerInfo;
    public static IMediator Mediator;
    public static I18nLookup I18n;
    public static IRandomNumberGenerator Random;
    public static IDateTimeService DateTime;
}


public static class Data
{
    public static T Get<T>(
        string name
    ) => default(T);
}