using System;
using EventHorizon.Game.Server.Zone.External.DateTimeService;

namespace EventHorizon.Game.Server.Zone.Core.DateTimeService
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}