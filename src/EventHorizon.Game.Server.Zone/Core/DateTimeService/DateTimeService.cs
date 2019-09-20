using System;
using EventHorizon.Zone.Core.Model.DateTimeService;

namespace EventHorizon.Game.Server.Zone.Core.DateTimeService
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}