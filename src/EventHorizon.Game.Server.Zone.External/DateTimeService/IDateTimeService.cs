using System;

namespace EventHorizon.Game.Server.Zone.External.DateTimeService
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
    }
}